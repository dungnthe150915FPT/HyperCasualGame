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
using BayatGames.SaveGameFree.Serializers;
using BayatGames.SaveGameFree;

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
        // stop game
        Application.Quit();
    }
    public BaseWeapon[] listWeapon;
    private void Setting()
    {
        listWeapon = SaveGame.Load<BaseWeapon[]>("WeaponConfigTest", new BaseWeapon[0], new SaveGameJsonSerializer());
        foreach (BaseWeapon weap in listWeapon)
        {
            Debug.Log(weap.NameDisplay);
        }
    }
    private void LoadGame()
    {
        BaseWeapon weap = new BaseWeapon();
        weap = SaveGame.Load<BaseWeapon>("weapon", new BaseWeapon(), new SaveGameJsonSerializer());
        Debug.Log(weap.NameDisplay);
        // remove weapon have index 0 in listWeapon array


        FileInfo[] files = SaveGame.GetFiles();
        foreach (FileInfo file in files)
        {
            Debug.Log("file: " + file.Name);
        }
        // Log to path of saved game
        Debug.Log(SaveGame.SavePath.ToString());
    }
    private void NewGame()
    {
        GenerateConfig();
    }
    private void GenerateConfig()
    {
        generateWeapons();
        //generateInventory();
    }

    private Inventory inventory = new Inventory();
    private void generateInventory()
    {
        List<BaseWeapon> newListWeapon = new List<BaseWeapon>();
        Inventory inventory = new Inventory();
        var jsonSerializer = new SaveGameJsonSerializer();
        listWeapon = SaveGame.Load<BaseWeapon[]>("WeaponConfigTest", new BaseWeapon[0], new SaveGameJsonSerializer());
        foreach (BaseWeapon weap in listWeapon)
        {
            Debug.Log(weap.NameDisplay);
            newListWeapon.Add(weap);
        }
        inventory.Weapons = newListWeapon;

        Debug.Log("newListWeapon: " + inventory.Weapons.Count);
        SaveGame.Save<Inventory>("Weapo", inventory, new SaveGameJsonSerializer());

    }

    private void generateWeapons()
    {
        BaseWeapon weap = new BaseWeapon();
        weap.Id = "1";
        weap.NameDisplay = "ACM";
        weap.SpritePath = "Sprites/Weapons/Guns/AR/ACM";
        weap.ShellExtractor = new Vector2(0.269f, 0.183f);
        weap.MuzzleExtractor = new Vector2(1.22f, 0.203f);
        weap.BulletSpeed = 1000f;
        weap.AttackDamage = 10;
        weap.FireRate = 0.1f;
        weap.ReloadTime = 0.5f;
        weap.SpreadAim = 0.1f;
        weap.Mass = 1;
        weap.AmmoMax = 30;
        weap.AmmoCurrent = 30;
        weap.MoveSpeedMultiplier = 1.2f;
        weap.JumpSpeedpMultiplier = 1.5f;
        weap.WeaponType = WeaponEnum.EWeaponType.Rifle;
        weap.AmmoType = WeaponEnum.EAmmoType.Rifle;
        weap.WeaponState = WeaponEnum.EWeaponState.Idle;
        // fire mode have single and auto
        WeaponEnum.EFireMode[] fir = new WeaponEnum.EFireMode[2];
        fir[0] = WeaponEnum.EFireMode.Single;
        fir[1] = WeaponEnum.EFireMode.Auto;
        weap.FireMode = fir;
        var jsonSerializer = new SaveGameJsonSerializer();

        BaseWeapon hp416 = new BaseWeapon();
        hp416.Id = "2";
        hp416.NameDisplay = "HP416";
        hp416.SpritePath = "Sprites/Weapons/Guns/AR/HP416";
        hp416.ShellExtractor = new Vector2(0.137f, 0.218f);
        hp416.MuzzleExtractor = new Vector2(1.22f, 0.203f);
        hp416.BulletSpeed = 1000f;
        hp416.AttackDamage = 10;
        hp416.FireRate = 0.1f;
        hp416.ReloadTime = 1f;
        hp416.SpreadAim = 0.1f;
        hp416.Mass = 1;
        hp416.AmmoMax = 30;
        hp416.AmmoCurrent = 30;
        hp416.MoveSpeedMultiplier = 1.2f;
        hp416.JumpSpeedpMultiplier = 1.5f;
        hp416.WeaponType = WeaponEnum.EWeaponType.Rifle;
        hp416.AmmoType = WeaponEnum.EAmmoType.Rifle;
        hp416.WeaponState = WeaponEnum.EWeaponState.Idle;
        hp416.FireMode = fir;

        BaseWeapon auc = new BaseWeapon();
        auc.Id = "3";
        auc.NameDisplay = "AUC";
        auc.SpritePath = "Sprites/Weapons/Guns/AR/AUC";
        auc.ShellExtractor = new Vector2(0.137f, 0.218f);
        auc.MuzzleExtractor = new Vector2(0.967f, 0.211f);
        auc.BulletSpeed = 1000f;
        auc.AttackDamage = 10;
        auc.FireRate = 0.1f;
        auc.ReloadTime = 0.25f;
        auc.SpreadAim = 0.1f;
        auc.Mass = 1;
        auc.AmmoMax = 30;
        auc.AmmoCurrent = 30;
        auc.MoveSpeedMultiplier = 1.2f;
        auc.JumpSpeedpMultiplier = 1.5f;
        auc.WeaponType = WeaponEnum.EWeaponType.Rifle;
        auc.AmmoType = WeaponEnum.EAmmoType.Rifle;
        auc.WeaponState = WeaponEnum.EWeaponState.Idle;
        auc.FireMode = fir;

        BaseWeapon[] listWeap = new BaseWeapon[3];
        listWeap[0] = weap;
        listWeap[1] = hp416;
        listWeap[2] = auc;

        SaveGame.Save<BaseWeapon[]>("WeaponConfigTest", listWeap, jsonSerializer);
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
