#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using Core.Score;
using Player;
using UI.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class UIManager : MonoBehaviour {
        private static PlayerHudView _playerHud;
        private static PauseMenuView _pauseMenu;
        private static GameOverView _gameOver;
        private static SettingsView _settingsView;

        private void Awake() {
            var sceneCanvases = GameObject.FindGameObjectsWithTag("Canvas");
            _ = sceneCanvases.First(canvas => canvas.TryGetComponent(out _pauseMenu));
            _ = sceneCanvases.First(canvas => canvas.TryGetComponent(out _gameOver));
            _ = sceneCanvases.First(canvas => canvas.TryGetComponent(out _settingsView));
            _ = sceneCanvases.First(canvas => canvas.TryGetComponent(out _playerHud));
            
            ScoreManager.Initialize();
            _playerHud.SetNewScore(0);
            OnHideCanvas();
        }

        private void Start() {
            _pauseMenu.HideCanvas();
            _gameOver.HideCanvas();
            _settingsView.HideCanvas();
        }

        private void OnEnable() {
            _pauseMenu.ResumePressed += HidePauseMenu;
            _pauseMenu.RestartPressed += RestartScene;
            _pauseMenu.ExitPressed += ExitApp;
            _pauseMenu.SettingsPressed += ShowSettings;

            _gameOver.RestartPressed += RestartScene;
            _gameOver.ExitPressed += ExitApp;
            PlayerController.Defeated += ShowGameOver;

            _settingsView.BackFromSettings += ReturnFromSettings;

            ScoreManager.ScoreChanged += ChangeScoreInHud;
        }

        private void OnDisable() {
            _pauseMenu.ResumePressed -= HidePauseMenu;
            _pauseMenu.RestartPressed -= RestartScene;
            _pauseMenu.ExitPressed -= ExitApp;
            _pauseMenu.SettingsPressed -= ShowSettings;
            
            _gameOver.RestartPressed -= RestartScene;
            _gameOver.ExitPressed -= ExitApp;
            
            _settingsView.BackFromSettings -= ReturnFromSettings;
            
            ScoreManager.ScoreChanged -= ChangeScoreInHud;
        }

        private void ChangeScoreInHud(int newValue) {
            _playerHud.SetNewScore(newValue);
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

        private static void HidePauseMenu() {
            if (_gameOver.IsEnabled) return;
            
            OnHideCanvas();
            _pauseMenu.HideCanvas();
        }

        private static void ShowGameOver() {
            ScoreManager.UpdateBestScore();
            _gameOver.SetScores(ScoreManager.CurrentScore, ScoreManager.BestScore);
            OnShowCanvas();
            ScoreManager.SaveBestScore();
            _gameOver.ShowCanvas();
        }

        private static void ShowSettings() {
            // NOTE: Since only access to settings is from pause menu (for now) it's hardcoded to return to it.
            _pauseMenu.HideCanvas();
            _settingsView.ShowCanvas();
        }

        private static void ReturnFromSettings() {
            // NOTE: Since only access to settings is from pause menu (for now) it's hardcoded to return to it.
            _settingsView.HideCanvas();
            _pauseMenu.ShowCanvas();
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