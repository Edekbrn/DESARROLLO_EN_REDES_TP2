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


    public void Shoot()
    {
        if(_CurrentTime < _WeaponInfo.FireRate) return;

        _CurrentTime = 0;

        if(!_WeaponInfo.UnlimitedAmmo) 
            _CurrentAmmunition--;

        float angle = Random.Range(-_WeaponInfo.SpreadAngle, _WeaponInfo.SpreadAngle);
        Quaternion spreadRot = Quaternion.AngleAxis(angle, Vector3.forward) * _BulletSpawn.rotation;


        Runner.Spawn(_WeaponInfo.BulletToSpawn, _BulletSpawn.position, spreadRot);
    }

    public bool AmmunitionIsOver() => _CurrentAmmunition <= 0;



}
