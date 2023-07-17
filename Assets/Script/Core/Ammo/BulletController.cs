using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletDamage = 10;
    public Collider2D trigger;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody2DBullet;

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
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
