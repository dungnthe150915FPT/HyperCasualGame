using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //private BaseBullet bulletStat;
    //public BaseBullet BulletStat
    //{
    //    get { return bulletStat; }
    //    set { bulletStat = value; }
    //}

    private float bulletDamage;

    //public Collider2D boxCollider2D;
    public Collider2D trigger;
    public SpriteRenderer spriteRenderer;
    public new Rigidbody2D rigidbody2D;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
       
    }

    private void OnEnable()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.tag == CONST.TAG_ENEMY)
        //{
        //    Debug.Log("Hit enemy");
        //}
    }

    internal float getBulletDamage()
    {
        return bulletDamage;
    }

    internal void setBulletDamage(float damage)
    {
        bulletDamage = damage;
    }
}
