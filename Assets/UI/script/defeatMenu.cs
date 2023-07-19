using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        Time.timeScale = 1;
    }
}
