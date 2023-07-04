using Assets.Script.Core.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    private BaseMagic baseMagic;
    public BaseMagic BaseMagic
    {
        get { return baseMagic; }
        set { baseMagic = value; }
    }

    public CapsuleCollider2D collider2d;
    public CapsuleCollider2D colliderTrigger;
    public new Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == CONST.TAG_PLAYER)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
