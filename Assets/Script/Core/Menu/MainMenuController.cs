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
    private Button btnLogin;


    private Slider slider;
    private Dropdown dropdown;


    private void Start()
    {
        btnContinue = GameObject.Find("btnContinue").GetComponent<Button>();
        btnNewGame = GameObject.Find("btnNewGame").GetComponent<Button>();
        btnLoadGame = GameObject.Find("btnLoadGame").GetComponent<Button>();
        btnSetting = GameObject.Find("btnSettings").GetComponent<Button>();
        btnQuit = GameObject.Find("btnQuit").GetComponent<Button>();
        btnLogin = GameObject.Find("btnLogin").GetComponent<Button>();

        btnContinue.onClick.AddListener(ContinueGame);
        btnNewGame.onClick.AddListener(NewGame);
        btnLoadGame.onClick.AddListener(LoadGame);
        btnSetting.onClick.AddListener(Setting);
        btnQuit.onClick.AddListener(QuitGame);
        btnLogin.onClick.AddListener(Login);

        
    }

    private void Login()
    {
        string clientPackage = "com.example.prmapplication";
        AndroidNativeFunctions.StartApp(clientPackage, false);
    }

    private void QuitGame()
    {
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
        StartCoroutine(LoadSceneMode(CONST.SCENE_HOME1));
        //GenerateConfig();
    }
    private void GenerateConfig()
    {
        generateWeapons();
        //generateInventory();
    }

    private void generateWeapons()
    {
        BaseWeapon[] listWeap = DefaultData.GetBaseWeapons();
        var jsonSerializer = new SaveGameJsonSerializer();
        SaveGame.Save<BaseWeapon[]>("WeaponConfigTest2.json", listWeap, jsonSerializer);
    }

    private void ContinueGame()
    {
        StartCoroutine(LoadSceneMode(CONST.SCENE_HOME1));
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