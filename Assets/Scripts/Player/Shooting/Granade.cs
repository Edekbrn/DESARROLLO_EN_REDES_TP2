using Fusion.Addons.Physics;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkRigidbody3D))]
public class Granade : NetworkBehaviour
{
    [SerializeField] byte _dmg = 40;
    [SerializeField] int _cooldownd = 2;
    [SerializeField] byte Fuerza = 40;
    TickTimer _lifeTimer = TickTimer.None;

    public override void Spawned()
    {
        GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(transform.forward * Fuerza, ForceMode.Impulse);
        if (HasStateAuthority)
        {
            _lifeTimer = TickTimer.CreateFromSeconds(Runner, _cooldownd);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!_lifeTimer.Expired(Runner)) return;
        DespawnObject();
    }

    void DespawnObject()
    {
        Runner.Despawn(Object);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !HasStateAuthority) return;

        if (other.TryGetComponent(out LifeHandler player))
        {
            
            player.TakeDamage(_dmg);
        }

        DespawnObject();
    }
}
