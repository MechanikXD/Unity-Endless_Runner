using UnityEngine;

namespace UI.View {
    public abstract class ViewBase : MonoBehaviour {
        private Canvas _thisCanvas;
        
        public bool IsEnabled => _thisCanvas.enabled;

        protected void Awake() {
            _thisCanvas = GetComponent<Canvas>();
        }

        protected void OnEnable() => SubscribeToEvents();
        protected void OnDisable() => UnsubscribeFromEvents();

        protected abstract void SubscribeToEvents();
        protected abstract void UnsubscribeFromEvents();
        
        public void ShowCanvas() => _thisCanvas.enabled = true;
        public void HideCanvas() => _thisCanvas.enabled = false;
    }
}