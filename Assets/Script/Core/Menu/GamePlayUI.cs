using Assets.Script.Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GamePlayUI : MonoBehaviour
{
    private Button btnFire;
    private Button btnSwitchWepPre;

    [Header("Events")]
    private GameEvent onFire;
    private GameEvent onSwitchWepPre;
    void Start()
    {
        btnFire = GameObject.Find(CONST.UI_COMPONENT_NAME_BTN_FIRE).GetComponent<Button>();
        btnSwitchWepPre = GameObject.Find(CONST.UI_COMPONENT_NAME_BTN_SWITCH_WEP_PRE).GetComponent<Button>();
        setupButtons();
        setupEvents();
    }

    private void setupEvents()
    {
        onFire = Resources.Load<GameEvent>(CONST.PATH_EVENT_FIRE);
        onSwitchWepPre = Resources.Load<GameEvent>(CONST.PATH_EVENT_SWITCH_WEAPON);

    }

    private void setupButtons()
    {
        btnFire.onClick.AddListener(Fire);
        btnSwitchWepPre.onClick.AddListener(SwitchWeaponPre);
    }

    private void SwitchWeaponPre()
    {
        onSwitchWepPre.Raise(this, "PATH_EVENT_SWITCH_WEAPON");
    }

    protected void Fire()
    {
        onFire.Raise(this, "FromGameplayUI");
        Debug.Log("Raise Fire");
    }

    void Update()
    {

    }

    //public void OnEventRaised(Component sender, object data)
    //{
    //    if (data is string) Debug.Log("OnEventRaised in UI: " + (string)data);
    //    else Debug.Log("OnEventRaised in UI");
    //}
}
