using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagicalTower.UI
{
    /// <summary>Shows the Game Over panel and restarts the scene via its button.</summary>
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _restartButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(Restart);
        }

        public void Show()
        {
            _panel.SetActive(true);
        }

        private void Restart()
        {
            Time.timeScale = 1f; // timeScale is not reset automatically across scene loads
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
