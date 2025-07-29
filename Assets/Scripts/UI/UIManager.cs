using System.Linq;
using Player;
using UI.Views;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class UIManager : MonoBehaviour {
        private static PauseMenuView _pauseMenu;
        private static GameOverView _gameOver;

        private void Awake() {
            var sceneCanvases = GameObject.FindGameObjectsWithTag("Canvas");
            _ = sceneCanvases.First(canvas => canvas.TryGetComponent(out _pauseMenu));
            _ = sceneCanvases.First(canvas => canvas.TryGetComponent(out _gameOver));
            
            OnHideCanvas();
        }

        private void Start() {
            _pauseMenu.HideCanvas();
            _gameOver.HideCanvas();
        }

        private void OnEnable() {
            _pauseMenu.ResumePressed += HidePauseMenu;
            _pauseMenu.RestartPressed += RestartScene;
            _pauseMenu.ExitPressed += ExitApp;

            _gameOver.RestartPressed += RestartScene;
            _gameOver.ExitPressed += ExitApp;
            PlayerController.Defeated += ShowGameOver;
        }

        private void OnDisable() {
            _pauseMenu.ResumePressed -= HidePauseMenu;
            _pauseMenu.RestartPressed -= RestartScene;
            _pauseMenu.ExitPressed -= ExitApp;
            
            _gameOver.RestartPressed -= RestartScene;
            _gameOver.ExitPressed -= ExitApp;
        }

        private static void OnShowCanvas() {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private static void OnHideCanvas() {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void ShowPauseMenu() {
            if (_gameOver.IsEnabled) return;
            
            OnShowCanvas();
            _pauseMenu.ShowCanvas();
        }

        private void HidePauseMenu() {
            if (_gameOver.IsEnabled) return;
            
            OnHideCanvas();
            _pauseMenu.HideCanvas();
        }

        public void ShowGameOver() {
            // TODO: Display current and best scores
            OnShowCanvas();
            _gameOver.ShowCanvas();
        }
        
        // TODO: Move somewhere where those methods belong to
        private void ExitApp() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else 
            Application.Quit();
#endif
        }

        private void RestartScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}