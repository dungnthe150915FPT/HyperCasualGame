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

    // Combat Properties
    private Inventory inventory;
    private WeaponController currentWepController; // current weapon controller
    private int currentWeaponIndex; // index of current weapon in inventory
    public GameObject weaponHand;   // hand hold weapon
    public GameObject weaponShell;  // shell of weapon
    public GameObject weaponMuzzle; // muzzle of weapon
    private bool isFaceRight = true;
    void Start()
    {
        // Setup Rigidbody
        setupRigidBody();

        // Setup Collider and Trigger
        setupCollider();

        // Setup Animator
        setupAnimator();

        // Setup Virtual Camera
        setupVirtualCamera();

        // Setup Inventory
        setupInventory();

        // Setup Weapon
        setupWeapon();

        // Setup Gameplay UI 
        setupGameplayUI();

        // Setup Events Listeners
        setupEventListeners();


    }

    private void setupEventListeners()
    {
        addEventListener(CONST.PATH_EVENT_FIRE, OnFire);
        addEventListener("Events/StopFireEvent", OnFireStop);
        addEventListener(CONST.PATH_EVENT_SWITCH_WEAPON, OnSwitchWeapon);
        addEventListener("Events/MoveEvent", OnMove);
        addEventListener("Events/JumpEvent", OnJump);
        addEventListener("Events/RunEvent", OnRun);
    }

    private void OnRun(Component sender, object data)
    {
        Debug.Log("Run");
    }

    private void OnJump(Component sender, object data)
    {
        Debug.Log("Run");
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
        animator.SetTrigger(CONST.ANIMATOR_TRIGGER_FIRE_SINGLE);

        currentWepController.OnFire(ResultGet, weaponHand);
    }

    // stop fire
    private void OnFireStop(Component sender, object data)
    {
        currentWepController.OnFireStop();
    }
    private void ResultGet(Component sender, object data)
    {
        if (data is string) Debug.Log("ResultGet: " + (string)data);
        else Debug.Log("ResultGet");


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
        GameObject gameplayUI = Instantiate(temp);
        gameplayUI.transform.SetParent(gameObject.transform);
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
    }
    private void setupInventory()
    {
        inventory = Inventory.Instance;
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
        transform.rotation = Quaternion.identity;
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
