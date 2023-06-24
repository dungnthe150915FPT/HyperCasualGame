using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Script.Core.Library;
using Assets.Script.Core.Weapon;
using UnityEngine.Windows;
using System.Runtime.Serialization.Json;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    private Button btnContinue;
    private Button btnNewGame;
    private Button btnLoadGame;
    private Button btnSetting;
    private Button btnQuit;

    private void Start()
    {
        btnContinue = GameObject.Find("btnContinue").GetComponent<Button>();
        btnNewGame = GameObject.Find("btnNewGame").GetComponent<Button>();
        btnLoadGame = GameObject.Find("btnLoadGame").GetComponent<Button>();
        btnSetting = GameObject.Find("btnSettings").GetComponent<Button>();
        btnQuit = GameObject.Find("btnQuit").GetComponent<Button>();

        btnContinue.onClick.AddListener(ContinueGame);
        btnNewGame.onClick.AddListener(NewGame);
        btnLoadGame.onClick.AddListener(LoadGame);
        btnSetting.onClick.AddListener(Setting);
        btnQuit.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        throw new NotImplementedException();
    }

    private void Setting()
    {
        throw new NotImplementedException();
    }

    private void LoadGame()
    {
        throw new NotImplementedException();
    }

    private void NewGame()
    {
        GenerateConfig();
    }

    private void GenerateConfig()
    {
        BaseWeapon weapon = new BaseWeapon();
        weapon.Id = "1";
        weapon.NameDisplay = "AK-47";
        weapon.AttackDamage = 10;
        weapon.FireRate = 0.1f;
        weapon.ReloadTime = 1;
        weapon.SpreadAim = 0.1f;
        weapon.Mass = 1;
        weapon.AmmoTotal = 30;
        weapon.AmmoCurrent = 0;
        weapon.MoveSpeedMultiplier = 1.2f;
        weapon.JumpSpeedpMultiplier = 1.5f;
        weapon.WeaponType = WeaponEnum.EWeaponType.Rifle;
        weapon.AmmoType = WeaponEnum.EAmmoType.Rifle;
        weapon.WeaponState = WeaponEnum.EWeaponState.Idle;

        string myWeaponJson = JsonUtility.ToJson(weapon);
        
    }

    private void ContinueGame()
    {
        StartCoroutine(LoadSceneMode(CONST.SCENE_TEST));
    }

    private IEnumerator LoadSceneMode(string nameScene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameScene);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading scene " + nameScene + " " + (asyncLoad.progress * 100) + "%");
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
