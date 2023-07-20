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
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    public Button btnNewGame;
    public Button btnLoadGame;
    public Button btnSetting;
    public Button btnQuit;
    public Button btnLogin;

    public UnityEvent nextBG;
    public float timeChangeBG = 3f;

    private Slider slider;
    private Dropdown dropdown;

    private float time = 0f;



    private void Start()
    {
        btnNewGame.onClick.AddListener(NewGame);
        //btnLoadGame.onClick.AddListener(LoadGame);
        btnSetting.onClick.AddListener(Setting);
        btnQuit.onClick.AddListener(QuitGame);
        btnLogin.onClick.AddListener(Login);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > timeChangeBG)
        {
            nextBG?.Invoke();
            time = 0f;
        }
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
    private void Setting()
    {
    }
    private void LoadGame()
    {
    }
    private void NewGame()
    {
        GenerateConfig();
        StartCoroutine(LoadSceneMode(CONST.SCENE_HOME1));
    }
    private void GenerateConfig()
    {
        generateWeapons();
    }

    private void generateWeapons()
    {
        BaseWeapon[] listWeap = DefaultData.GetBaseWeapons();
        var jsonSerializer = new SaveGameJsonSerializer();
        SaveGame.Save<BaseWeapon[]>(CONST.FILE_WEAPON_CONFIG, listWeap, jsonSerializer);
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