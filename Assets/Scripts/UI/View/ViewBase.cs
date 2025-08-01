using System;
using UnityEngine;

namespace UI.View {
    public abstract class ViewBase : MonoBehaviour {
        protected Canvas ThisCanvas;
        
        public bool IsEnabled => ThisCanvas.enabled;

        protected void Awake() {
            ThisCanvas = GetComponent<Canvas>();
        }

        protected void OnEnable() => SubscribeToEvents();
        protected void OnDisable() => UnsubscribeFromEvents();

        protected abstract void SubscribeToEvents();
        protected abstract void UnsubscribeFromEvents();
        
        public void ShowCanvas() => ThisCanvas.enabled = true;
        public void HideCanvas() => ThisCanvas.enabled = false;
    }
}