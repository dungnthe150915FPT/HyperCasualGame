using Assets.Script.Core.Character;
using Assets.Script.Core.Library;
using Assets.Script.Core.Weapon;
using BayatGames.SaveGameFree.Serializers;
using BayatGames.SaveGameFree;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static WeaponEnum;

public class CharacterControllerNew : MonoBehaviour
{
    // Unity Components
    private BaseCharacter character = new BaseCharacter();
    public new BoxCollider2D collider;
    public CapsuleCollider2D triggerCollider;
    public new Rigidbody2D rigidbody;
    private GameObject prefab;
    public SpriteRenderer weaponSpriteRenderer;
    public Animator animator;
    private Vector2 previousVelocity;
    private CinemachineVirtualCamera virtualCamera;
    private GameObject gameplayUIObject;
    public List<SpriteRenderer> listSpriteBone = new List<SpriteRenderer>();
    public List<SpriteRenderer> listSpriteHit = new List<SpriteRenderer>();

    // Combat Properties
    private GamePlayUI gameplayUI;
    private Inventory inventory;
    private WeaponController currentWepController; // current weapon controller
    private int currentWeaponIndex; // index of current weapon in inventory
    public GameObject weaponHand;   // hand hold weapon
    public GameObject weaponShell;  // shell of weapon
    public GameObject weaponMuzzle; // muzzle of weapon
    private bool isFaceRight = true;
    private bool isReloading = false;
    void Start()
    {
        // Setup Gameplay UI 
        setupGameplayUI();

        // Setup Rigidbody
        setupRigidBody();

        // Setup Collider and Trigger
        setupCollider();

        // Setup Animator
        setupAnimator();

        // Setup Virtual Camera
        setupVirtualCamera();

        // Setup Character Stats
        setupCharacterStats();

        // Setup Inventory
        setupInventory();

        // Setup Weapon
        setupWeapon();

        // Setup Events Listeners
        setupEventListeners();

    }

    private void setupCharacterStats()
    {
        character.MaxHealth = 100f;
        character.CurrentHealth = 100f;
        updateHealth();
    }

    private void updateHealth()
    {
        gameplayUI.updateHealthBar(character.CurrentHealth, character.MaxHealth);
    }

    private void setupEventListeners()
    {
        addEventListener(CONST.PATH_EVENT_FIRE, OnFire);
        addEventListener(CONST.PATH_EVENT_STOP_FIRE, OnFireStop);
        addEventListener(CONST.PATH_EVENT_SWITCH_WEAPON, OnSwitchWeapon);
        addEventListener(CONST.PATH_EVENT_MOVE, OnMove);
        addEventListener(CONST.PATH_EVENT_JUMP, OnJump);
        addEventListener(CONST.PATH_EVENT_RUN, OnRun);
        addEventListener(CONST.PATH_EVENT_RELOAD, OnReload);
    }
    private void OnReload(Component sender, object data)
    {
        if (getAmmoCurrWeap() <= 0)
        {
            return;
        }
        isReloading = true;
        animator.SetFloat(CONST.ANIMATOR_CONTROLLER_PARAMETER_RELOAD_MULTIPLIER,
            currentWepController.WeaponStat.ReloadTime);
        animator.SetTrigger(CONST.ANIMATOR_TRIGGER_RELOAD2);
        float reloadTime = (float)(CONST.ANIMATION_LENGTH_RELOAD2 *
            1 / currentWepController.WeaponStat.ReloadTime + CONST.ANIMATOR_LENGTH_EXIT_RELOAD2);
        StartCoroutine(StartReload(reloadTime));
    }

    private IEnumerator StartReload(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        int ammoPool = inventory.getAmmoCurrent(currentWepController.WeaponStat.AmmoType);
        currentWepController.OnReload(ReloadFinish, ammoPool);
    }

    private void ReloadFinish(Component sender, object result)
    {
        updateAmmoInventory((int)result);
        updateAmmoText(currentWepController.WeaponStat.AmmoCurrent);
        isReloading = false;
    }

    private int getAmmoCurrWeap()
    {
        return inventory.getAmmoCurrent(currentWepController.WeaponStat.AmmoType);
    }

    private void updateAmmoInventory(int result)
    {
        EAmmoType ammoType = currentWepController.WeaponStat.AmmoType;
        int ammoCurrent = getAmmoCurrWeap();
        inventory.setAmmoCurrent(ammoType, ammoCurrent > result ? ammoCurrent - result : 0);
    }

    private void OnRun(Component sender, object data)
    {
        Debug.Log("Run");
    }
    private void OnJump(Component sender, object data)
    {
        rigidbody.AddForce(Vector2.up * 50f, ForceMode2D.Impulse);
    }
    private void OnMove(Component sender, object data)
    {
        Vector2 vector2 = (Vector2)data;
        if (vector2 != Vector2.zero) startMoving(vector2);

    }
    private void stopMoving()
    {

    }
    private void startMoving(Vector2 vector)
    {
        onFaceSide(vector);
        setWeaponAngle(vector);

        rigidbody.velocity = new Vector2(vector.x * 10f, rigidbody.velocity.y);
        character.CharacterState = CharacterState.Walking;
        AnimatedLibrary.SetParameter(character.CharacterState, animator);
    }
    private void setWeaponAngle(Vector2 vector)
    {
        animator.SetFloat("aimDirection", (float)(vector.y) * 180f);
    }
    private void OnFire(Component sender, object data)
    {
        if (isReloading)
        {
            return;
        }
        if (currentWepController.OnFire(FireFinish))
        {
            animator.SetTrigger(CONST.ANIMATOR_TRIGGER_FIRE_SINGLE);
        }
    }
    private void OnFireStop(Component sender, object data)
    {
        currentWepController.OnFireStop();
    }
    private void FireFinish(Component sender, object data)
    {
        gameplayUI.setAmmoCurrentText(data.ToString());
    }
    private void OnSwitchWeapon(Component sender, object data)
    {
        changeWeapon(currentWeaponIndex + 1 >= inventory.getWeaponLength() ? 0 : currentWeaponIndex + 1);
    }
    private void addEventListener(string eventName, UnityAction<Component, object> callback)
    {
        GameEventListener eventListener = gameObject.AddComponent<GameEventListener>();
        GameEvent onInputEvent = Resources.Load<GameEvent>(eventName);
        eventListener.gameEvent = onInputEvent;
        eventListener.OnGameEventListenerEnable();
        CustomGameEvent eve = new CustomGameEvent();
        eve.AddListener(callback);
        eventListener.response = eve;
    }
    private void setupGameplayUI()
    {
        GameObject temp = Resources.Load<GameObject>(CONST.PREFAB_GAMEPLAY_UI);
        gameplayUIObject = Instantiate(temp);
        gameplayUIObject.transform.SetParent(gameObject.transform);
        gameplayUI = gameplayUIObject.GetComponent<GamePlayUI>();
    }
    private void setupWeapon()
    {
        currentWepController = gameObject.AddComponent<WeaponController>();
        BaseWeapon[] weapons = SaveGame.Load<BaseWeapon[]>("WeaponConfigTest", new BaseWeapon[0], new SaveGameJsonSerializer());

        inventory.addWeapon(weapons[0]);
        inventory.addWeapon(weapons[1]);
        inventory.addWeapon(weapons[2]);

        changeWeapon(1);
    }
    private void changeWeapon(int index)
    {
        currentWepController.WeaponStat = inventory.getWeapon(index);
        currentWepController.setupWeapon(weaponHand);
        inventory.setWeaponState(index, EWeaponState.Equipping);
        currentWeaponIndex = index;
        Sprite currentwepSprite = Resources.Load<Sprite>(currentWepController.WeaponStat.SpritePath);
        weaponSpriteRenderer.sprite = currentwepSprite;
        weaponShell.transform.localPosition = currentWepController.WeaponStat.ShellExtractor;
        weaponMuzzle.transform.localPosition = currentWepController.WeaponStat.MuzzleExtractor;

        updateAmmoText(currentWepController.WeaponStat.AmmoCurrent);
        updateImgWeap(currentWepController.WeaponStat.SpritePath);
    }

    private void updateImgWeap(string spritePath)
    {
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        gameplayUI.updateWeaponImage(sprite);
    }

    private void setupInventory()
    {
        inventory = Inventory.Instance;
        inventory.setAmmoPool(EAmmoType.Rifle, 999);
        inventory.setAmmoPool(EAmmoType.Shotgun, 999);
        inventory.setAmmoPool(EAmmoType.Pistol, 999);
        inventory.setAmmoCurrent(EAmmoType.Rifle, 123);
        inventory.setAmmoCurrent(EAmmoType.Shotgun, 123);
        inventory.setAmmoCurrent(EAmmoType.Pistol, 123);

    }

    private void updateAmmoText(int ammoCurrent)
    {
        gameplayUI.updateAmmo(ammoCurrent,
            inventory.getAmmoCurrent(currentWepController.WeaponStat.AmmoType));
    }

    private void setupVirtualCamera()
    {
        // find virtualCamera in hierarchy
        virtualCamera = GameObject.Find(CONST.VIRTUAL_CAMERA_NAME).GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null) Debug.Log("Virtual Camera not found. Creating new one.");

        virtualCamera.Follow = gameObject.transform;
        // set mode of virtual camera to orthographic
        virtualCamera.m_Lens.Orthographic = true;
        // set size orthographic of virtual camera
        virtualCamera.m_Lens.OrthographicSize = 10f;



        //virtualCamera.m_Lens.FieldOfView = 80;
        //virtualCamera.m_Lens.OrthographicSize = 3f;
        // body of virtual camera is Framing Transposer
        CinemachineFramingTransposer framingTrans = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        framingTrans.m_TrackedObjectOffset = new Vector3(5, 2, 0);
        framingTrans.m_XDamping = 0;
        framingTrans.m_YDamping = 0;
        framingTrans.m_ZDamping = 0;
    }
    private void setupAnimator()
    {
        previousVelocity = rigidbody.velocity;
        prefab = animator.gameObject;
        //throw new NotImplementedException();
    }
    private void setupCollider()
    {
        //throw new NotImplementedException();
    }
    private void setupRigidBody()
    {
        //throw new NotImplementedException();
    }
    void Update()
    {

    }
    private void FixedUpdate()
    {
        //transform.rotation = Quaternion.identity;
        calculationVector();
        idleVector();
    }
    private void idleVector()
    {
        if (character.IsInGround && !character.IsAccelerating)
        {
            character.CharacterState = CharacterState.Idle;
            AnimatedLibrary.SetParameter(CharacterState.Idle, animator);
        }
    }
    private void calculationVector()
    {
        Vector2 accelebration = rigidbody.velocity - previousVelocity;
        if (accelebration.magnitude > 0.1f)
        {
            character.IsAccelerating = true;
            animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_ACCELERATING, true);
        }
        else if (accelebration.magnitude < 0.1f)
        {
            character.IsAccelerating = false;
            animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_ACCELERATING, false);
        }

        // check gameobject is falling or not
        if (rigidbody.velocity.y < -0.5f && !character.IsInGround)
        {
            character.CharacterState = CharacterState.Falling;
            AnimatedLibrary.SetParameter(character.CharacterState, animator);
        }

        if (character.IsInGround && !character.IsAccelerating)
        {
            character.CharacterState = CharacterState.Idle;
            AnimatedLibrary.SetParameter(CharacterState.Idle, animator);
        }
    }

    void onFaceSide(Vector2 vector)
    {
        if (vector.x < 0 && isFaceRight)
        {
            prefab.transform.localScale = new Vector3(-1, 1, 1);
            isFaceRight = false;
        }
        else if (vector.x > 0 && !isFaceRight)
        {
            prefab.transform.localScale = new Vector3(1, 1, 1);
            isFaceRight = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == CONST.TAG_FLOOR)
        {
            animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_FALLING, false);
            character.IsInGround = true;
            animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_IN_GROUND, true);
        }
        if (collision.gameObject.tag == CONST.TAG_MAGIC)
        {
            BaseMagic mag = collision.gameObject.GetComponent<MagicController>().BaseMagic;
            OnHitByMagic(mag);
        }
    }

    private void OnHitByMagic(BaseMagic mag)
    {
        character.CurrentHealth -= mag.Damage;
        StartCoroutine(OnChangeCharacterColor(0.25f));
        updateHealth();
        if(character.CurrentHealth <= 0)
        {
            OnDeath();
        }

    }

    private void OnDeath()
    {

    }

    private IEnumerator OnChangeCharacterColor(float seconds)
    {
        changeBoneColor(Color.red);
        yield return new WaitForSeconds(seconds);
        changeBoneColor(Color.white);
    }

    private void changeBoneColor(Color red)
    {
        foreach (SpriteRenderer sprite in listSpriteHit)
        {
            sprite.color = red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == CONST.TAG_FLOOR)
        {
            if (rigidbody.velocity.y > 0.01)
            {
                character.CharacterState = CharacterState.Jumping;
                character.IsInGround = false;
                animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_IN_GROUND, false);
                AnimatedLibrary.SetParameter(character.CharacterState, animator);
            }
        }
    }
}
