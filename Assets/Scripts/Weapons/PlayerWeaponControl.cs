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

    void Update()
    {
        
    }

    public void Shoot()
    {
        if(_CurrentWeapon != null)
        {
            _CurrentWeapon.Shoot();
            if(_CurrentWeapon.AmmunitionIsOver())
            {
                // Despawn Object.

                
            }
            return;
        }

        DefaultWeapon.Shoot();

    }

    public void GetWeapon(Weapon newWeapon)
    {
        _CurrentWeapon = newWeapon;
    }
}
