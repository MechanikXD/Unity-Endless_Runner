using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views {
    public class PauseMenuView : MonoBehaviour {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        private Canvas _thisCanvas;
        private Action _unsubscribeFromEvents;

        public event Action ResumePressed;
        public event Action RestartPressed;
        public event Action ExitPressed;

        private void Awake() => _thisCanvas = GetComponent<Canvas>();

        private void OnEnable() {
            void OnResumePressed() => ResumePressed?.Invoke();
            void OnRestartPressed() => RestartPressed?.Invoke();
            void OnExitPressed() => ExitPressed?.Invoke();
            
            _resumeButton.onClick.AddListener(OnResumePressed);
            _restartButton.onClick.AddListener(OnRestartPressed);
            _exitButton.onClick.AddListener(OnExitPressed);

            _unsubscribeFromEvents = () => {
                _resumeButton.onClick.RemoveListener(OnResumePressed);
                _restartButton.onClick.RemoveListener(OnRestartPressed);
                _exitButton.onClick.RemoveListener(OnExitPressed);
            };
        }

        private void OnDisable() => _unsubscribeFromEvents();

        public void ShowCanvas() => _thisCanvas.enabled = true;
        public void HideCanvas() => _thisCanvas.enabled = false;
    }
}