using Assets.Script.Core.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Playgame()
    {
        SceneManager.LoadScene(CONST.SCENE_HOME1);
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}

