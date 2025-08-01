using TMPro;
using UnityEngine;

namespace UI.View.Views {
    public class PlayerHudView : MonoBehaviour {
        [SerializeField] private TMP_Text _currentScoreText;

        public void SetNewScore(int value) => _currentScoreText.SetText(value.ToString());
    }
}