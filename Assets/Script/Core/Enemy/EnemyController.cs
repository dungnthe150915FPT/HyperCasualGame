using Assets.Script.Core.Enemy;
using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static WeaponEnum;

public class EnemyController : MonoBehaviour
{
    private BaseEnemy enemy;
    public new Collider2D collider;
    public new Rigidbody2D rigidbody2D;
    private GameObject prefab;
    public Animator animator;
    public TextMeshPro textMeshPro;
    public GameObject layerUI;
    public PolygonCollider2D playerTrigger;
    public List<GameObject> toxinSpawnList;
    public bool isRandomToxinSpawn = true;
    public GameObject toxinExtractor;
    public GameObject movePointA;
    public GameObject movePointB;

    public bool isRandomRun = false;
    public bool isToxinToPlayer = false;

    // hide toxinDirection if isToxinToPlayer is false
    public Vector2 toxinDirection;

    private Vector3 positionPointA;
    private Vector3 positionPointB;

    private bool isAttacking = false;
    private bool isFaceRight = true;

    internal float moveSpeed;
    internal float maxHealth;
    internal float currentHealth;

    private void Start()
    {
        setupEnemy();
        setupToxin();
        removePositionMarker();
        setupAction();
    }


    public virtual BaseEnemy Enemy { get => enemy; set => enemy = value; }

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

    public virtual void setupEnemy()
    {
        enemy = new BaseEnemy();
        enemy.Id = "1";
        enemy.NameDisplay = "Wizard";
        enemy.MaxHealth = 100;
        enemy.CurrentHealth = 100;
        enemy.AttackDamage = 10;
        enemy.AttackSpeed = 1;
        enemy.AttackRange = 1;
        enemy.MoveSpeed = 5;
        enemy.JumpHeight = 1;

        setupEnemyController(enemy);
    }

    internal virtual void setupEnemyController(BaseEnemy enemy)
    {
        moveSpeed = enemy.MoveSpeed;
        maxHealth = enemy.MaxHealth;
        currentHealth = enemy.CurrentHealth;

        textMeshPro.text = enemy.NameDisplay;
    }

    private Vector3 target = Vector3.zero;
    internal virtual Vector3 Target { get => target; set => target = value; }
    internal virtual void setupAction()
    {
        //target = positionPointB;
        // target is the point A or B nearest to the wizard
        if (MathF.Abs(positionPointA.x - transform.position.x) < MathF.Abs(positionPointB.x - transform.position.x))
            Target = positionPointA;
        else Target = positionPointB;
    }

    // transform.rotation = Quaternion.Euler(0, 180, 0);

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
            //Debug.Log("Move speed: " + enemy.MoveSpeed);
            //Debug.Log("Crazy multiplier: " + crazyMultiplier);
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
        //return MathF.Abs(transform.position.x - target.x) < 1;
        // random distance to reach target
        float distanceToReach = 1;
        if (isRandomRun) distanceToReach = UnityEngine.Random.Range(
                MathF.Abs(positionPointA.x - positionPointB.x)
                / UnityEngine.Random.Range((float)2.5, 4), 1);
        return MathF.Abs(transform.position.x - Target.x) < distanceToReach;
    }

    internal virtual void moveByVelocity(float x)
    {
        //animator.SetBool("isRun", true);
        rigidbody2D.velocity = new Vector2(x, rigidbody2D.velocity.y);
    }

    internal virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBullet(collision)) OnHitByBullet(collision.gameObject);
    }

    internal virtual void OnHitByBullet(GameObject bullet)
    {
        float damage = bullet.GetComponent<BulletController>().getBulletDamage();
        Debug.Log("DamagetoEnemy: " + damage);

        currentHealth -= damage;
        Destroy(bullet);
        if (currentHealth <= 0) OnDie();
    }

    internal virtual void OnDie()
    {
        // set inactive game object
        gameObject.SetActive(false);
        // start coroutine to active game object after 5 seconds
        StartCoroutine(OnDieCoroutine(5));
    }

    private IEnumerator OnDieCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(true);
    }

    public virtual void shouldAttackPlayer(Component sender, object data)
    {
        GameObject player = ((Collider2D)data).gameObject;
        shouldRotateToPlayer(player);
        isAttacking = true;
        onAttackPlayer(player);
    }

    public virtual void playerOutOfSight(Component sender, object data)
    {
        isAttacking = false;
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
        if (toxinSpawnList.Count == 0)
        {
            Debug.Log("Toxin spawn list is empty");
            return;
        }
        foreach (GameObject toxinSpawn in toxinSpawnList)
        {
            BaseToxin newbaseToxin = new BaseToxin();
            newbaseToxin = toxinSpawn.GetComponent<ToxinController>().BaseToxin;
            baseToxinToSpawnList.Add(newbaseToxin);
        }
    }

    internal int countToxinSpawned = -1;
    internal List<BaseToxin> baseToxinToSpawnList = new List<BaseToxin>();

    internal virtual void spawnToxinToPlayer(GameObject player)
    {
        if (toxinSpawnList.Count == 0) return;
        int indexToSpawn = 0;
        if (isRandomToxinSpawn) indexToSpawn = UnityEngine.Random.Range(0, toxinSpawnList.Count);
        else
        {
            // spawn toxin in order of list
            countToxinSpawned++;
            if (countToxinSpawned >= toxinSpawnList.Count) countToxinSpawned = 0;
            indexToSpawn = countToxinSpawned;

        }
        GameObject toxinToSpawn = toxinSpawnList[indexToSpawn];
        BaseToxin baseToxin = baseToxinToSpawnList[indexToSpawn];
        GameObject toxin = Instantiate(toxinToSpawn, toxinExtractor.transform.position, Quaternion.identity);
        toxin.transform.Rotate(0, 0, -90);
        toxin.GetComponent<ToxinController>().BaseToxin = baseToxin;

        Vector2 direction = Vector2.zero;
        if (isToxinToPlayer)
        {
            Vector2 gamep = player.transform.position + new Vector3(0, UnityEngine.Random.Range(2, 5), 0);
            Vector2 magicp = toxinExtractor.transform.position + new Vector3(0, 0, 0);
            direction = gamep - magicp;
        }
        else direction = toxinDirection;
        toxin.GetComponent<Rigidbody2D>().AddForce(direction * baseToxin.Speed);
        Destroy(toxin, 2f);
    }
}
