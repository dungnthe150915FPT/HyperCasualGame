using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GamePlayUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Touchable")]
    public Button btnFire;
    public Button btnSwitchWepPre;
    public Button btnJump;
    public Button btnRun;
    public Button btnReload;
    private DynamicJoystick joystick;

    [Header("Text")]
    public TextMeshProUGUI textDebug;
    public TextMeshProUGUI textAmmoPool;
    public TextMeshProUGUI textAmmoCurrent;

    private GameEvent onFire;
    private GameEvent onStopFire;
    private GameEvent onSwitchWepPre;
    private GameEvent onMove;
    private GameEvent onJump;
    private GameEvent onRun;
    private GameEvent onReload;

    // EventTrigger
    private EventTrigger firee;
    private EventTrigger jump;

    // Parameter
    private float holdFireTime = 0f;
    void Start()
    {
        setupButtons();
        setupEvents();
        setupJoyStick();
    }
    private void setupJoyStick()
    {
        joystick = GameObject.Find("Dynamic Joystick").GetComponent<DynamicJoystick>();
    }
    private void setupEvents()
    {
        onFire = Resources.Load<GameEvent>(CONST.PATH_EVENT_FIRE);
        onStopFire = Resources.Load<GameEvent>(CONST.PATH_EVENT_STOP_FIRE);
        onSwitchWepPre = Resources.Load<GameEvent>(CONST.PATH_EVENT_SWITCH_WEAPON);
        onMove = Resources.Load<GameEvent>(CONST.PATH_EVENT_MOVE);
        onJump = Resources.Load<GameEvent>(CONST.PATH_EVENT_JUMP);
        onRun = Resources.Load<GameEvent>(CONST.PATH_EVENT_RUN);
        onReload = Resources.Load<GameEvent>(CONST.PATH_EVENT_RELOAD);
    }

    private void setupButtons()
    {
        firee = btnFire.AddComponent<EventTrigger>();
        EventTrigger.Entry entryFire = new EventTrigger.Entry();
        entryFire.eventID = EventTriggerType.PointerDown;
        entryFire.callback.AddListener((data) => { Fire(); });
        firee.triggers.Add(entryFire);
        entryFire = new EventTrigger.Entry();
        entryFire.eventID = EventTriggerType.PointerUp;
        entryFire.callback.AddListener((data) => { StopFire(); });
        firee.triggers.Add(entryFire);

        btnSwitchWepPre.onClick.AddListener(SwitchWeaponPre);
        btnReload.onClick.AddListener(ReloadAmmunation);

        jump = btnJump.AddComponent<EventTrigger>();
        EventTrigger.Entry entryJump = new EventTrigger.Entry();
        entryJump.eventID = EventTriggerType.PointerDown;
        entryJump.callback.AddListener((data) => { Jump(); });
        jump.triggers.Add(entryJump);
        entryJump = new EventTrigger.Entry();
        entryJump.eventID = EventTriggerType.PointerUp;
        entryJump.callback.AddListener((data) => { StopJump(); });
        jump.triggers.Add(entryJump);

        //btnRun.onClick.AddListener(Run);
    }

    private void ReloadAmmunation()
    {
        onReload.Raise(this, "PATH_EVENT_RELOAD");
    }

    private void StopJump()
    {
        Debug.Log("StopJump");
    }
    private void Run()
    {
        onRun.Raise(this, "PATH_EVENT_RUN");
    }
    private void Jump()
    {
        onJump.Raise(this, "PATH_EVENT_JUMP");
    }
    private void SwitchWeaponPre()
    {
        onSwitchWepPre.Raise(this, "PATH_EVENT_SWITCH_WEAPON");
    }
    private bool isFiring = false;
    protected void Fire()
    {
        onFire.Raise(this, "FromGameplayUI");
        isFiring = true;
    }
    private void StopFire()
    {
        onStopFire.Raise(this, "FromGameplayUI");
        isFiring = false;
        Debug.Log("StopFire");
    }
    void Update()
    {
    }
    private void FixedUpdate()
    {
        if (isFiring) holdFireTime += Time.fixedDeltaTime;
        else holdFireTime = 0f;
        onMove.Raise(this, joystick.Direction);
        if (isFiring && holdFireTime == 0) Fire();
        if (isFiring && holdFireTime > 0.05f) Fire();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ((IPointerDownHandler)btnFire).OnPointerDown(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ((IPointerUpHandler)btnFire).OnPointerUp(eventData);
    }
    public object setDebugText(string text)
    {
        textDebug.text = text;
        return null;
    }

    public object setAmmoPoolText(string text)
    {
        textAmmoPool.text = text;
        return null;
    }

    public object setAmmoCurrentText(string text)
    {
        textAmmoCurrent.text = text;
        return null;
    }

    internal void updateAmmo(int ammoCurrent, int ammoPool)
    {
        setAmmoCurrentText(ammoCurrent.ToString());
        setAmmoPoolText(ammoPool.ToString());
    }
}
