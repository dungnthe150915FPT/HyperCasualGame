using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMagic
{
    private float damage;
    private float speed;

    public float Damage { get; set; }
    public float Speed { get; set; }
    public BaseMagic(float damage, float speed)
    {
        Damage = damage;
        Speed = speed;
    }
    public BaseMagic() { }
}
