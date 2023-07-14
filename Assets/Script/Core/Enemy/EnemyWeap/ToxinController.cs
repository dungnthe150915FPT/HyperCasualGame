using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinController : MonoBehaviour
{
    public float damage;
    public float speed;

    public float Damage { get => damage; set => damage = value; }
    public float Speed { get => speed; set => speed = value; }

    internal virtual void Start()
    {

    }

    private void OnValidate()
    {

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
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
