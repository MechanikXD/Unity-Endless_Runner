using System;
using System.Linq;
using UI.Views;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class UIManager : MonoBehaviour {
        private static PauseMenuView _pauseMenu;
        // [SerializeField] private GameOverView _gameOver;

        private void Awake() {
            var sceneCanvases = GameObject.FindGameObjectsWithTag("Canvas");
            _ = sceneCanvases.First(canvas => canvas.TryGetComponent(out _pauseMenu));
            
            OnHideCanvas();
        }

        private void Start() {
            _pauseMenu.HideCanvas();
        }

        private void OnEnable() {
            _pauseMenu.ResumePressed += HidePauseMenu;
            _pauseMenu.RestartPressed += RestartScene;
            _pauseMenu.ExitPressed += ExitApp;
        }

        private void OnDisable() {
            _pauseMenu.ResumePressed -= HidePauseMenu;
            _pauseMenu.RestartPressed -= RestartScene;
            _pauseMenu.ExitPressed -= ExitApp;
        }

        private static void OnShowCanvas() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private static void OnHideCanvas() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void ShowPauseMenu() {
            // if (_gameOver.IsEnabled) return;
            
            OnShowCanvas();
            _pauseMenu.ShowCanvas();
        }

        private void HidePauseMenu() {
            // if (_gameOver.IsEnabled) return;
            
            OnHideCanvas();
            _pauseMenu.HideCanvas();
        }

        /*public void ShowGameOver(bool isEnabled) {
            OnShowCanvas();
            if (isEnabled) _gameOver.ShowCanvas();
            else _gameOver.HideCanvas();
        }*/
        
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