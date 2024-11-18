using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class DMG_Tranpa : NetworkBehaviour
{
    [SerializeField] byte _dmg = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !HasStateAuthority) return;

        if (other.TryGetComponent(out LifeHandler player))
        {
            player.TakeDamage(_dmg);
        }
    }
  
}
