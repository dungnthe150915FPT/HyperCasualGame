using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySight : MonoBehaviour
{
    public Collider2D detectTrigger;
    public GameObject enemy;
    public CustomGameEvent shouldAttackPlayer;
    public CustomGameEvent playerOutOfSight;
    public CustomGameEvent playerStayInSight;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer(collision))
        {
            shouldAttackPlayer?.Invoke(this, collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPlayer(collision))
        {
            playerStayInSight?.Invoke(this, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPlayer(collision))
        {
            playerOutOfSight?.Invoke(this, collision);
        }
    }

    private bool isPlayer(Collider2D collision)
    {
        return collision is BoxCollider2D && collision.gameObject.tag == CONST.TAG_PLAYER;
    }
}
