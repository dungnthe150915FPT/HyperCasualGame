using Assets.Script.Core.Library;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.script
{
    public class MainMenu : MonoBehaviour
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
}
