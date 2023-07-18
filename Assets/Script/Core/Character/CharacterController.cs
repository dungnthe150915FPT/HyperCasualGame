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
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using System.Reflection;

public class CharacterController : MonoBehaviour
{
    // Unity Components
    private BaseCharacter character = new BaseCharacter();
    public BoxCollider2D colliderCharacter;
    public Collider2D triggerCollider;
    public Rigidbody2D rigidbodyCharacter;
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
    private AudioClip pickupWeaponAudio;
    private bool isFaceRight = true;
    private bool isReloading = false;
    private bool isRunning = false;
    private bool isSwitchingWeapon = false;
    private float speedMultiplier = 1f;

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

        // Setup Audio
        setupAudio();

        // Setup Character Stats
        setupCharacterStats();

        // Setup Inventory
        setupInventory();

        // Setup Weapon
        setupWeapon();

        // Setup Events Listeners
        setupEventListeners();

    }

    private void setupAudio()
    {
        pickupWeaponAudio = Resources.Load<AudioClip>(CONST.SOUND_HEAL_PATH);
    }

    private void setupCharacterStats()
    {
        character.MoveSpeed = 10f;
        character.MaxHealth = 100f;
        character.CurrentHealth = 100f;
        character.JumpHeight = 50f;
        speedMultiplier = 1f;
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
        addEventListener(CONST.PATH_EVENT_RUN_STOP, OnRunStop);
        addEventListener(CONST.PATH_EVENT_RELOAD, OnReload);
        addEventListener(CONST.PATH_EVENT_PICKUP_OBJECT, OnPickUpObject);
    }

    private void OnReload(Component sender, object data)
    {
        if (getAmmoCurrWeap() <= 0 || isReloading || currentWepController.WeaponStat.AmmoCurrent == currentWepController.WeaponStat.AmmoMax) return;
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
        if (character.State == CharacterState.Walking)
        {
            isRunning = true;
            speedMultiplier = 3f;
            character.State = CharacterState.Running;
        }
        AnimatedLibrary.SetParameter(character.State, animator);
    }

    private void OnRunStop(Component arg0, object arg1)
    {
        if (character.State == CharacterState.Running)
        {
            isRunning = false;
            speedMultiplier = 1f;
            character.State = CharacterState.Walking;
        }
    }
    private void OnJump(Component sender, object data)
    {
        rigidbodyCharacter.AddForce(Vector2.up * character.JumpHeight, ForceMode2D.Impulse);
    }
    private void OnMove(Component sender, object data)
    {
        Vector2 vector2 = (Vector2)data;
        if (vector2 != Vector2.zero) startMoving(vector2);
    }
    private void stopMoving()
    {
        if (isRunning) character.State = CharacterState.Idle;
        else character.State = CharacterState.Idle;
        AnimatedLibrary.SetParameter(character.State, animator);
    }
    private void startMoving(Vector2 vector)
    {
        onFaceSide(vector);
        setWeaponAngle(vector);
        rigidbodyCharacter.velocity = new Vector2(vector.x * character.MoveSpeed * speedMultiplier, rigidbodyCharacter.velocity.y);
        if (isRunning) character.State = CharacterState.Running;
        else character.State = CharacterState.Walking;
        AnimatedLibrary.SetParameter(character.State, animator);
    }
    private void setWeaponAngle(Vector2 vector)
    {
        animator.SetFloat("aimDirection", (float)(vector.y) * 180f);
    }
    private void OnFire(Component sender, object data)
    {
        if (isReloading || isPickuping || isSwitchingWeapon) return;
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
        if (isSwitchingWeapon) return;
        StartCoroutine(OnBlankToAim(0.2f));
    }

    private IEnumerator OnBlankToAim(float v)
    {
        // stop animation current playing, then play blank to aim
        isSwitchingWeapon = true;
        animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_AIMING, false);
        yield return new WaitForSeconds(v);
        animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_AIMING, true);
        changeWeapon(getIndexToEquip());
        gameplayUI.changeWeapImageToSwitch(getImageToEquipNext());
        isSwitchingWeapon = false;
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
        gameplayUI.changeWeapImageToSwitch(getImageToEquipNext());
    }
    private Sprite getImageToEquipNext()
    {
        int index = getIndexToEquip();
        return Resources.Load<Sprite>(inventory.getWeapon(index).SpritePath);
    }
    private int getIndexToEquip()
    {
        return currentWeaponIndex + 1 >= inventory.getWeaponLength() ? 0 : currentWeaponIndex + 1;
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
        inventory.setAmmoPool(EAmmoType.Circle, 999);
        inventory.setAmmoPool(EAmmoType.Round, 999);
        inventory.setAmmoPool(EAmmoType.Sharp, 999);
        inventory.setAmmoCurrent(EAmmoType.Circle, 888);
        inventory.setAmmoCurrent(EAmmoType.Round, 888);
        inventory.setAmmoCurrent(EAmmoType.Sharp, 888);

    }
    private void updateAmmoText(int ammoCurrent)
    {
        gameplayUI.updateAmmo(ammoCurrent,
            inventory.getAmmoCurrent(currentWepController.WeaponStat.AmmoType));
    }
    private void setupVirtualCamera()
    {
        virtualCamera = GameObject.Find(CONST.VIRTUAL_CAMERA_NAME).GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null) Debug.Log("Virtual Camera not found. Creating new one.");

        virtualCamera.Follow = gameObject.transform;
        virtualCamera.m_Lens.Orthographic = true;
        virtualCamera.m_Lens.OrthographicSize = 10f;
        virtualCamera.m_Lens.OrthographicSize = 8f;
        CinemachineFramingTransposer framingTrans = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        framingTrans.m_TrackedObjectOffset = new Vector3(5, 2, 0);
        framingTrans.m_XDamping = 0;
        framingTrans.m_YDamping = 0;
        framingTrans.m_ZDamping = 0;
    }
    private void setupAnimator()
    {
        previousVelocity = rigidbodyCharacter.velocity;
        prefab = animator.gameObject;
        animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_AIMING, true);
    }
    private void setupCollider()
    {
    }
    private void setupRigidBody()
    {
    }
    void Update()
    {

    }
    private void FixedUpdate()
    {
        calculationVector();
        idleVector();
    }
    private void idleVector()
    {
        if (character.IsInGround && !character.IsAccelerating)
        {
            character.State = CharacterState.Idle;
            AnimatedLibrary.SetParameter(CharacterState.Idle, animator);
        }
    }
    private void calculationVector()
    {
        Vector2 accelebration = rigidbodyCharacter.velocity - previousVelocity;
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
        if (rigidbodyCharacter.velocity.y < -0.5f && !character.IsInGround)
        {
            character.State = CharacterState.Falling;
            AnimatedLibrary.SetParameter(character.State, animator);
        }

        if (character.IsInGround && !character.IsAccelerating)
        {
            character.State = CharacterState.Idle;
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
    }

    public void OnHitByToxin(GameObject toxin)
    {
        character.CurrentHealth -= toxin.GetComponent<ToxinController>().Damage;
        StartCoroutine(OnChangeCharacterColor(0.25f));
        updateHealth();
        if (character.CurrentHealth <= 0) OnDeath();
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
            if (rigidbodyCharacter.velocity.y > 0.01)
            {
                character.State = CharacterState.Jumping;
                character.IsInGround = false;
                animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_IN_GROUND, false);
                AnimatedLibrary.SetParameter(character.State, animator);
            }
        }
    }

    internal void MeetObjectPickup(GameObject gameObject, object data)
    {
        gameplayUI.showObjectPickup(true);
        switch (data)
        {
            case BaseWeapon weapon:
                Sprite sprite = Resources.Load<Sprite>(weapon.SpritePath);
                string name = weapon.NameDisplay;
                gameplayUI.changeObjectPickup(sprite, name);
                weaponToPickup = weapon;
                objectCollisionNow = gameObject;
                break;
        }
    }

    private BaseWeapon weaponToPickup;
    private GameObject objectCollisionNow;

    internal void ExitObjectPickup()
    {
        gameplayUI.showObjectPickup(false);
    }
    private void OnPickUpObject(Component sender, object data)
    {
        if (weaponToPickup == null) return;
        AudioSource.PlayClipAtPoint(pickupWeaponAudio, transform.position);
        if (inventory.getWeaponLength() < inventory.getNumOfWeaponSlot())
        {
            StartCoroutine(OnSetPickupStatus(0.5f));
            inventory.addWeapon(weaponToPickup);
            gameplayUI.changeWeapImageToSwitch(getImageToEquipNext());
            Destroy(objectCollisionNow);
        }
        else if (inventory.getWeaponLength() == inventory.getNumOfWeaponSlot())
        {
            spawnObjectPickup(currentWepController.WeaponStat);
            inventory.removeWeapon(currentWeaponIndex);
            StartCoroutine(OnSetPickupStatus(1.3f));
            int index = inventory.addWeapon(weaponToPickup);
            changeWeapon(index);
            Destroy(objectCollisionNow);
        }
    }

    private IEnumerator OnSetPickupStatus(float v)
    {
        animator.SetTrigger("On_Pickup");
        isPickuping = true;
        yield return new WaitForSeconds(v);
        isPickuping = false;
    }

    private bool isPickuping = false;

    private void spawnObjectPickup(BaseWeapon weaponStat)
    {
        GameObject gameObject = Instantiate(Resources.Load<GameObject>(CONST.PREFAB_WEAPON_PICKUP_PATH));
        gameObject.GetComponent<WeaponPickup>().setupByPlayer(weaponStat);
        if (!isFaceRight) gameObject.transform.localScale = new Vector3(-1, 1, 1);
        gameObject.transform.position = weaponHand.gameObject.transform.position;
        gameObject.GetComponent<WeaponPickup>().nameWeapon = weaponStat.NameDisplay;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -2), ForceMode2D.Impulse);
    }
}