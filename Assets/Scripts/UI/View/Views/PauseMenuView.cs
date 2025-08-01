using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Views {
    public class PauseMenuView : ViewBase {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        private Action _unsubscribeFromEvents;

        public event Action ResumePressed;
        public event Action RestartPressed;
        public event Action SettingsPressed;
        public event Action ExitPressed;

        protected override void SubscribeToEvents() {
            void OnResumePressed() => ResumePressed?.Invoke();
            void OnRestartPressed() => RestartPressed?.Invoke();
            void OnExitPressed() => ExitPressed?.Invoke();
            void OnSettingsPressed() => SettingsPressed?.Invoke();
            
            _resumeButton.onClick.AddListener(OnResumePressed);
            _restartButton.onClick.AddListener(OnRestartPressed);
            _exitButton.onClick.AddListener(OnExitPressed);
            _settingsButton.onClick.AddListener(OnSettingsPressed);

            _unsubscribeFromEvents = () => {
                _resumeButton.onClick.RemoveListener(OnResumePressed);
                _restartButton.onClick.RemoveListener(OnRestartPressed);
                _exitButton.onClick.RemoveListener(OnExitPressed);
                _settingsButton.onClick.RemoveListener(OnSettingsPressed);
            };
        }

        protected override void UnsubscribeFromEvents() => _unsubscribeFromEvents();
    }
}