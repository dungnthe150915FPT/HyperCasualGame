using Assets.Script.Core.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Home()
    {
        SceneManager.LoadScene(CONST.SCENE_MAIN_MENU2);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

    }

    public void Restart()
    {
        SceneManager.LoadScene(CONST.SCENE_MAIN_MENU2);
        Time.timeScale = 1;
    }
}
