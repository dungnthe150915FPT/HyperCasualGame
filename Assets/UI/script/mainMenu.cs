using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.script
{
    public class MainMenu : MonoBehaviour
    {

        public void Playgame()
        {
            SceneManager.LoadScene("play");
        }

        public void Quitgame()
        {
            Application.Quit();
        }
            
    }
}
