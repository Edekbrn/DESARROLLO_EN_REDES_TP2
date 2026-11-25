using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWeaponControl : NetworkBehaviour
{
    [SerializeField] Weapon DefaultWeapon;
    [SerializeField] Transform SocketWeapon;

    Weapon _CurrentWeapon;

    float _RateToShoot;

    void Start()
    {
        
        
    }

    public void Shoot(Vector3 AimPoint)
    {
        if(_CurrentWeapon != null)
        {
            _CurrentWeapon.Shoot(AimPoint);
            if(_CurrentWeapon.AmmunitionIsOver())
            {
                // Despawn Object.

                
            }
            return;
        }

        DefaultWeapon.Shoot(AimPoint);

    }

    public void GetWeapon(Weapon newWeapon)
    {
        _CurrentWeapon = newWeapon;
    }
}
