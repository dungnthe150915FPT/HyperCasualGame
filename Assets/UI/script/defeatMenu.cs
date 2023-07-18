using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.script
{
    public class DefeatMenu : MonoBehaviour
    {
        public void Home()
        {
            SceneManager.LoadScene("UI/scene/MainMenu");
        }
        public void Restart()
        {
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
            Time.timeScale = 1;
        }
    }
}
