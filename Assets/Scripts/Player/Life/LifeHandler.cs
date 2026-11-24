using Fusion;
using System.Collections;
using System;
using UnityEngine;

public class LifeHandler : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(CurrentLifeChanged))]
    byte CurrentLife { get; set; }

    const byte MAX_LIFE = 100;

    byte _maxDeaths = 3;

    [Networked, OnChangedRender(nameof(DeadStateChanged))]
    NetworkBool IsDead { get; set; }

    LifeBarItem _lifeBarItem;

    public event Action<bool> OnDeadStateChanged = delegate { };
    public event Action OnResurrect = delegate { };
    public event Action OnDespawn = delegate { };


    bool _HasLimitedLives;
    int _CurrentLive;
    int _MaxLives;
    [Networked] public NetworkBool NoLives { get; set; }

    public override void Spawned()
    {
        _lifeBarItem = LifeBarHandler.Instance.CreateNewLifeBarItem(this);

        if (HasStateAuthority)
        {
            CurrentLife = MAX_LIFE;
        }
        else
        {
            CurrentLifeChanged();
            DeadStateChanged();
        }
    }

    public void SetLives(int count)
    {
        _HasLimitedLives = true;
        _MaxLives = count;
        _CurrentLive = _MaxLives;
        
    }

    public void TakeDamage(byte dmg)
    {
        if (IsDead) return;

        if (CurrentLife < dmg)
        {
            dmg = CurrentLife;
        }
        CurrentLife -= dmg;

        if (CurrentLife > 0) return;

        if(_HasLimitedLives)
        {
            _CurrentLive--;
            if(_CurrentLive <= 0)
            {
                NoLives = true;
                GameManager.Instance.LivesMode_OnPlayerEliminated(Object.InputAuthority);
                //Runner.Disconnect(Object.InputAuthority);
                Runner.Despawn(Object);
                return;
            }
        }

        IsDead = true;
        StartCoroutine(Server_ResurrectCooldown());
    }

    IEnumerator Server_ResurrectCooldown()
    {
        yield return new WaitForSeconds(2);
        Server_Resurrect();
    }

    void Server_Resurrect()
    {
        OnResurrect();
        CurrentLife = MAX_LIFE;
        IsDead = false;
    }

    void DeadStateChanged()
    {
        GetComponentInParent<HitboxRoot>().HitboxRootActive = !IsDead;
        OnDeadStateChanged(IsDead);
    }

    void CurrentLifeChanged()
    {
        _lifeBarItem.UpdateFillAmount(CurrentLife / (float)MAX_LIFE);
    }

    
    void EliminatePlayer()
    {
        if (!Object.HasInputAuthority)
        {
            Runner.Disconnect(Object.InputAuthority);
        }


        // Mandar esto a gamemanager
        Runner.Despawn(Object);
        Death();
    }


    // Improve
    void DisconnectPlayer()
    {

        Runner.Despawn(Object);
        Death();
    }


    // CHECK
    void Death()
    {
        if (_maxDeaths == 0)
        {
            GameManager.Instance.Defeat();
        }
        else
        {
            if (Runner.LocalPlayer != Object.InputAuthority)
            {
                RPC_Win(Object.InputAuthority);
            }
        }

        Runner.Despawn(Object);
    }



    // CHECK
    [Rpc]
    public void RPC_Win(PlayerRef playerRef)
    {
        if (playerRef == Runner.LocalPlayer)
        {
            Debug.Log("You Win!");
            GameManager.Instance.TriggerWin();
        }
        else
        {
            Debug.Log("You Lose");
        }
    }


    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawn();
    }
}


