using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WizardEneController : MonoBehaviour
{
    private WizardEnemy wizardEnemy;
    public new BoxCollider2D collider;
    public CapsuleCollider2D triggerCollider;
    public new Rigidbody2D rigidbody2D;
    private GameObject prefab;
    public Animator animator;
    public TextMeshPro textMeshPro;
    public PolygonCollider2D playerTrigger;
    public GameObject magicBullet;

    private bool isGrounded = false;
    private bool isAttacking = false;

    private void Start()
    {
        setupWizard();
    }

    private void setupWizard()
    {
        wizardEnemy = new WizardEnemy();
        wizardEnemy.Id = "1";
        wizardEnemy.NameDisplay = "Wizard";
        wizardEnemy.MaxHealth = 100;
        wizardEnemy.CurrentHealth = 100;
        wizardEnemy.AttackDamage = 10;
        wizardEnemy.AttackSpeed = 1;
        wizardEnemy.AttackRange = 1;
        wizardEnemy.MoveSpeed = 1;
        wizardEnemy.JumpHeight = 1;

        textMeshPro.text = wizardEnemy.NameDisplay;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer(collision))
        {
            isAttacking = true;
            onAttackPlayer(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPlayer(collision))
        {
            isAttacking = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPlayer(collision))
        {
            isAttacking = true;
        }
    }

    private bool isPlayer(Collider2D collision)
    {
        return collision is CapsuleCollider2D && collision.gameObject.tag == CONST.TAG_PLAYER;
    }

    private void onAttackPlayer(GameObject gameObject)
    {
        if (isAttacking)
        {
            StartCoroutine(attackPlayerCoroutine(0.5f, gameObject));
        }
    }

    private IEnumerator attackPlayerCoroutine(float seconds, GameObject gameObject)
    {
        // is is attacking = true then play animation in loop
        // if is is attacking = false then stop animation and stop coroutine
        if (isAttacking)
        {
            startAttackPlayer(gameObject);
            yield return new WaitForSeconds(seconds);
            StartCoroutine(attackPlayerCoroutine(seconds, gameObject));
        }
        else
        {
            stopAttackPlayer();
            StopCoroutine(attackPlayerCoroutine(seconds, gameObject));
        }
    }

    private void stopAttackPlayer()
    {
        animator.SetTrigger("idle");
    }

    private void startAttackPlayer(GameObject gameObject)
    {
        spawnMagic(gameObject);
        animator.SetTrigger("attack");
    }

    private void spawnMagic(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
