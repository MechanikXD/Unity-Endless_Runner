#if UNITY_EDITOR
using UnityEditor;
#endif
using Core.Score;
using Player;
using UI.View.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class UIManager : MonoBehaviour {
        [SerializeField] private PlayerHudView _playerHud;
        
        [SerializeField] private PauseMenuView _pauseMenu;
        [SerializeField] private GameOverView _gameOver;
        [SerializeField] private SettingsView _settingsView;

        private void Awake() => Initialize();

        private void Start() => HideAllCanvases();

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

            PlayerController.PauseButtonPressed += ShowPauseMenu;
        }

        private void OnDisable() {
            _pauseMenu.ResumePressed -= HidePauseMenu;
            _pauseMenu.RestartPressed -= RestartScene;
            _pauseMenu.ExitPressed -= ExitApp;
            _pauseMenu.SettingsPressed -= ShowSettings;
            
            _gameOver.RestartPressed -= RestartScene;
            _gameOver.ExitPressed -= ExitApp;
            PlayerController.Defeated -= ShowGameOver;
            
            _settingsView.BackFromSettings -= ReturnFromSettings;
            
            ScoreManager.ScoreChanged -= ChangeScoreInHud;
            
            PlayerController.PauseButtonPressed -= ShowPauseMenu;
        }
        
        private void HideAllCanvases() {
            OnHideCanvas();
            _pauseMenu.HideCanvas();
            _gameOver.HideCanvas();
            _settingsView.HideCanvas();
        }

        private void Initialize() {
            ScoreManager.Initialize();
            _playerHud.SetNewScore(0);
        }

        private void ChangeScoreInHud(int newValue) => _playerHud.SetNewScore(newValue);

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

        private void ShowPauseMenu() {
            if (_gameOver.IsEnabled) return;
            
            OnShowCanvas();
            _pauseMenu.ShowCanvas();
        }

        private void HidePauseMenu() {
            if (_gameOver.IsEnabled) return;
            
            OnHideCanvas();
            _pauseMenu.HideCanvas();
        }

        private void ShowGameOver() {
            ScoreManager.UpdateBestScore();
            _gameOver.SetScores(ScoreManager.CurrentScore, ScoreManager.BestScore);
            OnShowCanvas();
            ScoreManager.SaveBestScore();
            _gameOver.ShowCanvas();
        }

        private void ShowSettings() {
            // NOTE: Since only access to settings is from pause menu (for now) it's hardcoded to return to it.
            _pauseMenu.HideCanvas();
            _settingsView.ShowCanvas();
        }

        private void ReturnFromSettings() {
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

        private void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}