using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.script
{
    public class VictoryMenu : MonoBehaviour
    {
        public void Home()
        {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1;
        }
        public void Restart()
        {
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
            Time.timeScale = 1;
        }

        public void nextMap()
        {
            // Kiểm tra nếu còn màn chơi tiếp theo
            if (HasNextLevel())
            {
                // Lấy tên cảnh của màn chơi tiếp theo
                string nextLevelName = GetNextLevelName();

                // Chuyển đến cảnh màn chơi tiếp theo
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                // Nếu không còn màn chơi tiếp theo, quay về màn hình chính
                SceneManager.LoadScene("MainMenu");
            }
        }

        private bool HasNextLevel()
        {
            // Kiểm tra xem còn màn chơi tiếp theo hay không
            // Ví dụ: Kiểm tra xem có một cảnh với tên "Level2" hay không
            // Đảm bảo thay đổi kiểm tra phù hợp với cách bạn tổ chức màn chơi trong trò chơi của bạn
            return SceneManager.GetSceneByName("Level2") != null;
        }

        private string GetNextLevelName()
        {
            // Lấy tên của màn chơi tiếp theo
            // Ví dụ: Trả về tên cảnh "Level2"
            // Đảm bảo thay đổi giá trị trả về phù hợp với tên cảnh mà bạn muốn chuyển đến
            return "Level2";
        }
    }
}
