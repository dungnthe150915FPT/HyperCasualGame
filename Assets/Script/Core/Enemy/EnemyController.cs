using Assets.Script.Core.Enemy;
using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Collider2D colliderEnemy;
    public Rigidbody2D rigidbody2DEnemy;
    private GameObject prefab;
    public Animator animator;
    public TextMeshPro tmp_name;
    public GameObject layerUI;
    public Slider healthBar;
    public PolygonCollider2D playerTrigger;
    public GameObject toxinSpawn;
    public bool isSpawnToxin = false;
    public GameObject toxinExtractor;
    public GameObject movePointA;
    public GameObject movePointB;
    private Image healthImageFill;

    public bool isRandomRun = false;
    public bool isToxinToPlayer = false;

    public Vector2 toxinDirection;

    private Vector3 positionPointA;
    private Vector3 positionPointB;

    private bool isAttacking = false;
    private bool isFaceRight = true;

    internal float moveSpeed;
    internal float maxHealth;
    internal float currentHealth;
    internal string nameDisplay;

    private GameEvent onLogDebug;
    private void Start()
    {
        setupToxin();
        setupEnemyController();
        removePositionMarker();
        setupAction();
        onLogDebug = Resources.Load<GameEvent>(CONST.PATH_DEBUG_EVENT);
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

    internal virtual void setupEnemyController()
    {
        healthImageFill = healthBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        tmp_name.text = nameDisplay;
        UpdateHealth(0);
    }

    private Vector3 target = Vector3.zero;
    internal virtual Vector3 Target { get => target; set => target = value; }
    internal virtual void setupAction()
    {
        if (MathF.Abs(positionPointA.x - transform.position.x) < MathF.Abs(positionPointB.x - transform.position.x))
            Target = positionPointA;
        else Target = positionPointB;
    }

    internal virtual void FixedUpdate()
    {
        if (!isAttacking) move();
    }

    internal virtual void move()
    {
        if (isAttacking) return;
        if (isTargetReached()) changeTarget();
        else moveToTarget();
    }
    internal virtual void moveToTarget()
    {
        if (Target.x > transform.position.x)
        {
            moveByVelocity(moveSpeed);
            if (!isFaceRight) rotateToRight(true);
        }
        else
        {
            moveByVelocity(-moveSpeed);
            if (isFaceRight) rotateToRight(false);
        }
    }

    internal virtual void rotateToRight(bool isRight)
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

    internal virtual void changeTarget()
    {
        if (Target == positionPointA) Target = positionPointB;
        else Target = positionPointA;
    }

    internal virtual bool isTargetReached()
    {
        float distanceToReach = 1;
        if (isRandomRun) distanceToReach = UnityEngine.Random.Range(
                MathF.Abs(positionPointA.x - positionPointB.x)
                / UnityEngine.Random.Range((float)2.5, 4), 1);
        return MathF.Abs(transform.position.x - Target.x) < distanceToReach;
    }

    internal virtual void moveByVelocity(float x)
    {
        rigidbody2DEnemy.velocity = new Vector2(x, rigidbody2DEnemy.velocity.y);
    }

    internal virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBullet(collision)) OnHitByBullet(collision.gameObject);
    }

    internal virtual void OnHitByBullet(GameObject bullet)
    {
        float damage = bullet.GetComponent<BulletController>().bulletDamage;
        UpdateHealth(damage);
        Destroy(bullet);
        if (currentHealth <= 0) OnDie();
    }

    internal virtual void playAnimation(string animation, string type)
    {
        switch (type)
        {
            case "trigger":
                animator.SetTrigger(animation);
                break;
            case "bool":
                animator.SetBool(animation, true);
                break;
        }

    }

    private void UpdateHealth(float damage)
    {
        healthBar.value = currentHealth = currentHealth -= damage;
        float fillAmount = currentHealth / maxHealth;
        Color color = new Color();
        color.a = healthImageFill.color.a;
        if (fillAmount > 0.5f) color = Color.Lerp(Color.yellow, Color.green, (fillAmount - 0.5f) * 2);
        else color = Color.Lerp(Color.red, Color.yellow, fillAmount * 2);
        healthImageFill.color = color;
        if (currentHealth < 5) healthImageFill.enabled = false;
    }

    internal virtual void OnDie()
    {
        isAttacking = false;
        StartCoroutine(OnDieCoroutine(2));
    }

    private IEnumerator OnDieCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        layerUI.SetActive(false);
        gameObject.SetActive(false);
    }

    public virtual void shouldAttackPlayer(Component sender, object data)
    {
        GameObject player = ((Collider2D)data).gameObject;
        shouldRotateToPlayer(player);
        isAttacking = true;
        onAttackPlayer(player);
        string log = gameObject.name + "  attack player  " + player.name;
        onLogDebug.Raise(this, log);
    }

    public virtual void playerOutOfSight(Component sender, object data)
    {
        isAttacking = false;
        onLogDebug.Raise(this, "");
    }

    public virtual void playerStayInSight(Component sender, object data)
    {
        isAttacking = true;
    }

    internal virtual void OnTriggerExit2D(Collider2D collision)
    {

    }

    internal virtual void OnTriggerStay2D(Collider2D collision)
    {

    }

    internal virtual void shouldRotateToPlayer(GameObject player)
    {
        if (player.transform.position.x > transform.position.x)
            rotateToRight(true);
        else rotateToRight(false);
    }

    internal virtual bool isBullet(Collider2D collision)
    {
        return collision.gameObject.tag == CONST.TAG_BULLET;
    }

    internal virtual void onAttackPlayer(GameObject gameObject)
    {
        if (isAttacking)
        {
            StartCoroutine(attackPlayerCoroutine(0.5f, gameObject));
        }
    }

    internal virtual IEnumerator attackPlayerCoroutine(float seconds, GameObject gameObject)
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

    internal virtual void stopAttackPlayer()
    {
    }

    internal virtual void startAttackPlayer(GameObject player)
    {
        spawnToxinToPlayer(player);
    }

    internal virtual void setupToxin()
    {
    }

    internal virtual void spawnToxinToPlayer(GameObject player)
    {
        if (toxinSpawn == null || !isSpawnToxin) return;
        GameObject toxin = Instantiate(toxinSpawn, toxinExtractor.transform.position, Quaternion.identity);
        toxin.transform.Rotate(0, 0, -90);
        Vector2 direction = Vector2.zero;
        if (isToxinToPlayer)
        {
            Vector2 gamep = player.transform.position + new Vector3(0, UnityEngine.Random.Range(2, 5), 0);
            Vector2 magicp = toxinExtractor.transform.position + new Vector3(0, 0, 0);
            direction = gamep - magicp;
        }
        else direction = toxinDirection;
        toxin.GetComponent<Rigidbody2D>().AddForce(direction * toxin.GetComponent<ToxinController>().Speed);
        Destroy(toxin, 2f);
    }
}
