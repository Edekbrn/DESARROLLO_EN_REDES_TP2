using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Weapons")]
public class WeaponsScriptable : ScriptableObject
{
    [Header("Shoot paramaters")]
    public int Damage;
    public float FireRate;
    public int MaxAmmunition;
    public float SpreadAngle;
    public bool UnlimitedAmmo;
    public NetworkPrefabRef BulletToSpawn;


}
