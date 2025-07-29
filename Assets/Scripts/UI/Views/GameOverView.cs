using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views {
    public class GameOverView : MonoBehaviour {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _bestScore;
        [SerializeField] private TMP_Text _currentScore;
        private Canvas _thisCanvas;
        private Action _unsubscribeFromEvents;
        public bool IsEnabled => _thisCanvas.enabled;
        
        public event Action RestartPressed;
        public event Action ExitPressed;

        private void Awake() => _thisCanvas = GetComponent<Canvas>();

        private void OnEnable() {
            void OnRestartPressed() => RestartPressed?.Invoke();
            void OnExitPressed() => ExitPressed?.Invoke();
            
            _restartButton.onClick.AddListener(OnRestartPressed);
            _exitButton.onClick.AddListener(OnExitPressed);

            _unsubscribeFromEvents = () => {
                _restartButton.onClick.RemoveListener(OnRestartPressed);
                _exitButton.onClick.RemoveListener(OnExitPressed);
            };
        }

        private void OnDisable() => _unsubscribeFromEvents();

        public void SetScores(int currentScore, int bestScore) {
            _currentScore.SetText(currentScore.ToString());
            _bestScore.SetText(bestScore.ToString());
        }
        
        public void ShowCanvas() => _thisCanvas.enabled = true;
        public void HideCanvas() => _thisCanvas.enabled = false;
    }
}