using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : NetworkBehaviour
{
    [SerializeField] WeaponsScriptable _WeaponInfo;
    [SerializeField] Transform _BulletSpawn;

    int _CurrentAmmunition = 2;

    float _CurrentTime;

    void Start()
    {
        if(!_WeaponInfo.UnlimitedAmmo)
            _CurrentAmmunition = _WeaponInfo.MaxAmmunition;
        
        _CurrentTime = _WeaponInfo.FireRate;
    }

    void Update()
    {
        if(_CurrentTime < _WeaponInfo.FireRate) _CurrentTime += Time.deltaTime;
    }

    // Vector3 = controller.AimPoint 
    public void Shoot(Vector3 aimPoint)
    {
        if(_CurrentTime < _WeaponInfo.FireRate) return;

        _CurrentTime = 0;

        if(!_WeaponInfo.UnlimitedAmmo) 
            _CurrentAmmunition--;

        // Dirección precisa: de la punta del arma al mouse
        Vector3 dir = aimPoint - _BulletSpawn.position;
        dir.z = 0f;                                   // mantener la bala en el plano 2.5D

        // Si el mouse está encima de la punta del arma, usa hacia donde mira el arma
        if (dir.sqrMagnitude < 0.01f)
            dir = _BulletSpawn.forward;

        dir.Normalize();

        // Dispersión opcional sobre la dirección ya apuntada
        float angle = Random.Range(-_WeaponInfo.SpreadAngle, _WeaponInfo.SpreadAngle);
        dir = Quaternion.AngleAxis(angle, Vector3.forward) * dir;

        Runner.Spawn(_WeaponInfo.BulletToSpawn, _BulletSpawn.position, Quaternion.LookRotation(dir));

    }

    public bool AmmunitionIsOver() => _CurrentAmmunition <= 0;



}
