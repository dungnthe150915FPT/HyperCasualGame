using Assets.Script.Core.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private BaseAmmo ammoStat;
    public BaseAmmo AmmoStat { get; set; }

    public BoxCollider2D boxCollider2D;
    public BoxCollider2D boxTrigger;
    public SpriteRenderer spriteRenderer;
    public new Rigidbody2D rigidbody2D;

    public bool isMoving = false;
    private void Start()
    {
    }

    private void FixedUpdate()
    {
        //Debug.Log("isMoving: " + isMoving.ToString());
        //rigidbody2D.velocity = transform.up * 1000;

    }

    private void OnEnable()
    {
        //boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        //boxTrigger = gameObject.AddComponent<BoxCollider2D>();
        //boxTrigger.isTrigger = true;
        //spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        //rigidbody2D = gameObject.AddComponent<Rigidbody2D>();

        //spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Weapons/BulletHead/circle bullet");
        // scale bullet to 10x
        //transform.localScale = new Vector3(10, 10, 10);
        //rigidbody2D.mass = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("BulletController: OnTriggerEnter2D");
        //if (collision.gameObject.tag == CONST.TAG_PLAYER)
        //{
        //    Debug.Log("BulletController: OnTriggerEnter2D: Player");
        //}
        //else
        //{
        //    isMoving = false;
        //}
    }
}
