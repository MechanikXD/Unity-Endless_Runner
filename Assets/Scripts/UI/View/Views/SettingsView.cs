using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Views {
    public class SettingsView : ViewBase {
        [SerializeField] private Button _backButton;
        private Action _unsubscribeFromEvents;

        public event Action BackFromSettings;

        protected override void SubscribeToEvents() {
            void OnBackFromSettings() => BackFromSettings?.Invoke();
            
            _backButton.onClick.AddListener(OnBackFromSettings);

            _unsubscribeFromEvents = () => {
                _backButton.onClick.RemoveListener(OnBackFromSettings);
            };
        }

        protected override void UnsubscribeFromEvents() => _unsubscribeFromEvents();
    }
}