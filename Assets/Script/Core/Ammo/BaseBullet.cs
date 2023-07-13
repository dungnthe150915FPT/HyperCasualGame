using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponEnum;

public class BaseBullet
{
    private float damage;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public BaseBullet() { }
    public BaseBullet(float damage)
    {
        this.damage = damage;
    }
}
