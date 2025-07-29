using UnityEngine;

namespace UI.Views {
    public class GameOverView : MonoBehaviour {
        private Canvas _thisCanvas;
        public bool IsEnabled => _thisCanvas.enabled;

        private void Awake() {
            _thisCanvas = GetComponent<Canvas>();
        }
        
        public void ShowCanvas() => _thisCanvas.enabled = true;
        public void HideCanvas() => _thisCanvas.enabled = false;
    }
}