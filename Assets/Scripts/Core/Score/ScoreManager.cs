using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Score {
    public static class ScoreManager {
        private static int _currentScore;
        private static int _bestScore;
        private const string ScorePrefKey = "Score";

        private static Action _unsubscribeFromEvents;

        public static void Initialize() {
            _currentScore = 0;
            LoadBestScore();
            void SaveScoreOnSceneUnload(Scene arg) {
                SaveBestScore();
                SceneManager.sceneUnloaded -= SaveScoreOnSceneUnload;
            }
            SceneManager.sceneUnloaded += SaveScoreOnSceneUnload;
        }

        public static void AddScore(int value) {
            if (value > 0) _currentScore += value;
        }
        
        private static void LoadBestScore() {
            _bestScore = PlayerPrefs.HasKey(ScorePrefKey) ? PlayerPrefs.GetInt(ScorePrefKey) : 0;
        }

        private static void SaveBestScore() {
            if (_currentScore > _bestScore) PlayerPrefs.SetInt(ScorePrefKey, _currentScore);
        }
    }
}