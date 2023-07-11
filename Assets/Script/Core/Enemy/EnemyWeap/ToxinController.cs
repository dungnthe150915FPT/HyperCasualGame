using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinController : MonoBehaviour
{
    private BaseToxin baseToxin;
    public BaseToxin BaseToxin
    {
        get { return baseToxin; }
        set { baseToxin = value; }
    }

    public Collider2D collider2d;
    public Collider2D colliderTrigger;
    public new Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;

    public float damage;
    public float speed;

    internal virtual void Start()
    {

    }

    private void OnValidate()
    {
        setupToxin();
    }

    private void setupToxin()
    {
        BaseToxin newbaseToxin = new BaseToxin();
        newbaseToxin.Speed = speed;
        newbaseToxin.Damage = damage;
        baseToxin = newbaseToxin;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == CONST.TAG_PLAYER)
        {
            collision.gameObject.GetComponent<CharacterController>().OnHitByToxin(gameObject);
            Destroy(gameObject);
        }
    }
}
