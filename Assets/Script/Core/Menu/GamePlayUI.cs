using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GamePlayUI : MonoBehaviour
{
    private Button btnFire;
    void Start()
    {
        btnFire = GameObject.Find("btnFire").GetComponent<Button>();
        setupButtons();
    }

    private void setupButtons()
    {
        btnFire.onClick.AddListener(Fire);
    }

    protected void Fire()
    {
        Debug.Log("Fire in UI");
    }

    void Update()
    {

    }

    public void OnEventRaised(Component sender, object data)
    {
        if (data is string) Debug.Log("OnEventRaised in UI: " + (string)data);
        else Debug.Log("OnEventRaised in UI");
    }
}
