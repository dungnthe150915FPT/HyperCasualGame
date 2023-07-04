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
    public new Rigidbody2D rigidbody2D;
    private GameObject prefab;
    public Animator animator;
    public TextMeshPro textMeshPro;
    public GameObject layerUI;
    public PolygonCollider2D playerTrigger;
    public GameObject magicBullet;
    public GameObject magicExtractor;
    public GameObject movePointA;
    public GameObject movePointB;
    public bool isRandomRun = false;

    private Vector3 positionPointA;
    private Vector3 positionPointB;
    private float crazyMultiplier = 1f;

    private bool isAttacking = false;
    private bool isFaceRight = true;

    private void Start()
    {
        setupWizard();
        removePositionMarker();
        setupAction();
    }



    private void OnValidate()
    {
        positionPointA = gameObject.transform.position;
        positionPointB = gameObject.transform.position;
    }

    private void removePositionMarker()
    {
        positionPointA = movePointA.transform.position;
        positionPointB = movePointB.transform.position;

        Destroy(movePointA);
        Destroy(movePointB);

        // if (positionPointA.x > positionPointB.x) replace position
        if (positionPointA.x > positionPointB.x)
        {
            Vector3 temp = positionPointA;
            positionPointA = positionPointB;
            positionPointB = temp;
        }
        else
        {
            Vector3 temp = positionPointA;
            positionPointA = positionPointB;
            positionPointB = temp;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(positionPointA, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(positionPointB, 0.5f);
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
        wizardEnemy.MoveSpeed = 5;
        wizardEnemy.JumpHeight = 1;

        textMeshPro.text = wizardEnemy.NameDisplay;
    }

    private Vector3 target = Vector3.zero;
    private void setupAction()
    {
        //target = positionPointB;
        // target is the point A or B nearest to the wizard
        if (MathF.Abs(positionPointA.x - transform.position.x) < MathF.Abs(positionPointB.x - transform.position.x))
            target = positionPointA;
        else target = positionPointB;
    }

    // transform.rotation = Quaternion.Euler(0, 180, 0);

    private void FixedUpdate()
    {
        if (!isAttacking) move();
    }

    private void move()
    {
        if (isAttacking) return;
        if (isTargetReached()) changeTarget();
        else moveByTarget();
    }
    private void moveByTarget()
    {
        if (target.x > transform.position.x)
        {
            moveByVelocity(wizardEnemy.MoveSpeed * crazyMultiplier);
            if (!isFaceRight) rotateToRight(true);
        }
        else
        {
            moveByVelocity(-wizardEnemy.MoveSpeed + crazyMultiplier);
            if (isFaceRight) rotateToRight(false);
        }
    }

    private void rotateToRight(bool isRight)
    {
        if (isRight)
        {
            isFaceRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            isFaceRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        layerUI.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void changeTarget()
    {
        if (target == positionPointA) target = positionPointB;
        else target = positionPointA;
    }

    private bool isTargetReached()
    {
        //return MathF.Abs(transform.position.x - target.x) < 1;
        // random distance to reach target
        float distanceToReach = 1;
        if (isRandomRun) distanceToReach = UnityEngine.Random.Range(
                MathF.Abs(positionPointA.x - positionPointB.x)
                / UnityEngine.Random.Range((float)2.5, 4), 1);
        return MathF.Abs(transform.position.x - target.x) < distanceToReach;
    }

    private void moveByVelocity(float x)
    {
        animator.SetBool("isRun", true);
        rigidbody2D.velocity = new Vector2(x, rigidbody2D.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer(collision))
        {
            // rotate to player
            if (collision.gameObject.transform.position.x > transform.position.x)
                rotateToRight(true);
            else rotateToRight(false);
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
        GameObject magic = Instantiate(
            magicBullet,
            magicExtractor.transform.position,
            Quaternion.identity);
        magic.transform.Rotate(0, 0, -90);

        BaseMagic baseMagic = new BaseMagic();
        baseMagic.Damage = 5f;
        baseMagic.Speed = 80f;
        magic.GetComponent<MagicController>().BaseMagic = baseMagic;

        Vector2 gamep = gameObject.transform.position + new Vector3(0, UnityEngine.Random.Range(2, 5), 0);
        Vector2 magicp = magicExtractor.transform.position + new Vector3(0, 0, 0);
        Vector2 direction = gamep - magicp;
        magic.GetComponent<Rigidbody2D>().AddForce(direction * baseMagic.Speed);
        Destroy(magic, 2f);
    }
}
