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
    public DynamicJoystick joystick;
    public TextMeshProUGUI txtDebug;
    private Button btnFire;
    private Button btnSwitchWepPre;
    private Button btnJump;
    private Button btnRun;

    [Header("Events")]
    private GameEvent onFire;
    private GameEvent onStopFire;
    private GameEvent onSwitchWepPre;
    private GameEvent onMove;
    private GameEvent onJump;
    private GameEvent onRun;
    void Start()
    {
        btnFire = GameObject.Find(CONST.UI_COMPONENT_NAME_BTN_FIRE).GetComponent<Button>();
        btnSwitchWepPre = GameObject.Find(CONST.UI_COMPONENT_NAME_BTN_SWITCH_WEP_PRE).GetComponent<Button>();
        btnJump = GameObject.Find(CONST.UI_COMPONENT_NAME_BTN_JUMP).GetComponent<Button>();
        setupButtons();
        setupEvents();
        setupJoyStick();
    }

    private void setupJoyStick()
    {

    }

    private void setupEvents()
    {
        onFire = Resources.Load<GameEvent>(CONST.PATH_EVENT_FIRE);
        onStopFire = Resources.Load<GameEvent>("Events/StopFireEvent");
        onSwitchWepPre = Resources.Load<GameEvent>(CONST.PATH_EVENT_SWITCH_WEAPON);
        onMove = Resources.Load<GameEvent>("Events/MoveEvent");
        onJump = Resources.Load<GameEvent>("Events/JumpEvent");
        onRun = Resources.Load<GameEvent>("Events/RunEvent");

    }

    private EventTrigger firee;
    private EventTrigger jump;
    private float holdFireTime = 0f;
    private void setupButtons()
    {
        //btnFire.onClick.AddListener(Fire);
        firee = btnFire.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { Fire(); });
        firee.triggers.Add(entry);
        // point up
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { StopFire(); });
        firee.triggers.Add(entry);

        btnSwitchWepPre.onClick.AddListener(SwitchWeaponPre);

        //btnJump.onClick.AddListener(Jump);

        jump = btnJump.AddComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerDown;
        entry2.callback.AddListener((data) => { Jump(); });
        jump.triggers.Add(entry2);
        // point up
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerUp;
        entry3.callback.AddListener((data) => { StopJump(); });
        jump.triggers.Add(entry3);

        //btnRun.onClick.AddListener(Run);
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
        //Debug.Log("isFiring: " + isFiring + ": holdFire: " + holdFireTime);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ((IPointerDownHandler)btnFire).OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((IPointerUpHandler)btnFire).OnPointerUp(eventData);
    }
    //public void OnEventRaised(Component sender, object data)
    //{
    //    if (data is string) Debug.Log("OnEventRaised in UI: " + (string)data);
    //    else Debug.Log("OnEventRaised in UI");
    //}
}
