using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views {
    public class SettingsView : MonoBehaviour {
        [SerializeField] private Button _backButton;
        private Canvas _thisCanvas;
        private Action _unsubscribeFromEvents;

        public event Action BackFromSettings;
        
        private void Awake() => _thisCanvas = GetComponent<Canvas>();

        private void OnEnable() {
            void OnBackFromSettings() => BackFromSettings?.Invoke();
            
            _backButton.onClick.AddListener(OnBackFromSettings);

            _unsubscribeFromEvents = () => {
                _backButton.onClick.RemoveListener(OnBackFromSettings);
            };
        }

        private void OnDisable() => _unsubscribeFromEvents();
        
        public void ShowCanvas() => _thisCanvas.enabled = true;
        public void HideCanvas() => _thisCanvas.enabled = false;
    }
}