using Fusion.Addons.Physics;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkRigidbody3D))]
public class Granade : NetworkBehaviour
{
    [SerializeField] byte _dmg = 40;
    [SerializeField] int _cooldownd = 2;
    [SerializeField] int _cooldownDestroy = 4;
    [SerializeField] byte Fuerza = 40;
    [SerializeField] Collider _explosionColl;


    public event Action ThrowGranadeA = delegate { };
    TickTimer _lifeTimer = TickTimer.None;
    TickTimer _lifeTimer2 = TickTimer.None;



    public override void Spawned()
    {
        Vector3 fuerzaCombinada = (-transform.forward + Vector3.up).normalized * Fuerza;
        GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(fuerzaCombinada, ForceMode.VelocityChange);
        // GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(Vector3.up * Fuerza, ForceMode.VelocityChange);

        if (HasStateAuthority)
        {
            _lifeTimer = TickTimer.CreateFromSeconds(Runner, _cooldownd);
            _lifeTimer2 = TickTimer.CreateFromSeconds(Runner, _cooldownDestroy);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            if (_lifeTimer.Expired(Runner) && !_explosionColl.enabled)
            {
                _explosionColl.enabled = true;
            }

            if (_lifeTimer2.Expired(Runner))
            {
                DespawnObject();
            }
        }
        // if (!_lifeTimer.Expired(Runner)) return;
        // _explosionColl.enabled = true;
        // if (!_lifeTimer2.Expired(Runner)) return;
        // DespawnObject();
    }

    void DespawnObject()
    {
        Runner.Despawn(Object);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !HasStateAuthority) return;

        if (other.TryGetComponent(out LifeHandler player)|| _explosionColl.enabled==true)
        {

            player.TakeDamage(_dmg);
            explosion();
            _explosionColl.enabled = false;
        }
    }
    void explosion()
    {
        ThrowGranadeA();
        _explosionColl.enabled = false;
        DespawnObject();
    }
}
