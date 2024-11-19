using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(ShotHandler))]
[RequireComponent(typeof(LifeHandler))]
public class PlayerController : NetworkBehaviour
{
    NetworkCharacterControllerCustom _movementHandler;
    ShotHandler _shotHandler;
    private bool _canThrowGranade = true;
    [SerializeField] int _cooldowndGranade = 2;
    TickTimer _lifeTimer = TickTimer.None;

    public override void Spawned()
    {
        _movementHandler = GetComponent<NetworkCharacterControllerCustom>();
        _shotHandler = GetComponent<ShotHandler>();

        var lifeHandler = GetComponent<LifeHandler>();

        lifeHandler.OnDeadStateChanged += (b) =>
        {
            enabled = !b;
        };

        lifeHandler.OnResurrect += () =>
        {
            _movementHandler.Teleport(transform.position + Vector3.up * 2.5f);
        };
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData inputs)) return;

        //Movimiento
        var direction = Vector3.forward * inputs.movementInput;
        _movementHandler.Move(direction);

        //Disparo
        if (inputs.isFirePressed)
        {
            _shotHandler.Fire();
        }

        //Salto
        if (inputs.networkButtons.IsSet(MyButtons.Jump))
        {
            _movementHandler.Jump();
        }

        //Granada
        if (inputs.isgranade && _canThrowGranade)
        {
            ThrowGranadeWithCooldown();
           // _shotHandler.ThrowGranade();
        }
    }
    private void ThrowGranadeWithCooldown()
    {
        _canThrowGranade = false;
        _shotHandler.ThrowGranade();
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, _cooldowndGranade);

        StartCoroutine(ResetGranadeCooldown());
    }

    private IEnumerator ResetGranadeCooldown()
    {
        while (!_lifeTimer.Expired(Runner))
        {
            yield return null;
        }

        _canThrowGranade = true;
    }
}
