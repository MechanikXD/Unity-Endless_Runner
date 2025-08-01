using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Views {
    public class GameOverView : ViewBase {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _bestScore;
        [SerializeField] private TMP_Text _currentScore;
        private Action _unsubscribeFromEvents;
        
        public event Action RestartPressed;
        public event Action ExitPressed;
        
        public void SetScores(int currentScore, int bestScore) {
            _currentScore.SetText(currentScore.ToString());
            _bestScore.SetText(bestScore.ToString());
        }

        protected override void SubscribeToEvents() {
            void OnRestartPressed() => RestartPressed?.Invoke();
            void OnExitPressed() => ExitPressed?.Invoke();
            
            _restartButton.onClick.AddListener(OnRestartPressed);
            _exitButton.onClick.AddListener(OnExitPressed);

            _unsubscribeFromEvents = () => {
                _restartButton.onClick.RemoveListener(OnRestartPressed);
                _exitButton.onClick.RemoveListener(OnExitPressed);
            };
        }

        protected override void UnsubscribeFromEvents() => _unsubscribeFromEvents();
    }
}