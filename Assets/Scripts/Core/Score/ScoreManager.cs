using System;
using UnityEngine;

namespace Core.Score {
    public static class ScoreManager {
        public static int CurrentScore { get; private set; }
        public static int BestScore { get; private set; }

        private static Action _unsubscribeFromEvents;

        public static event Action<int> ScoreChanged;

        public static void Initialize() {
            CurrentScore = 0;
            LoadBestScore();
        }

        public static void AddScore(int value) {
            if (value > 0) CurrentScore += value;
            
            ScoreChanged?.Invoke(CurrentScore);
        }
        
        private static void LoadBestScore() {
            BestScore = PlayerPrefs.HasKey("Score") ? PlayerPrefs.GetInt("Score") : 0;
        }

        public static void SaveBestScore() {
            UpdateBestScore();
            PlayerPrefs.SetInt("Score", CurrentScore > BestScore ? CurrentScore : BestScore);
        }

        public static void UpdateBestScore() {
            if (CurrentScore > BestScore) BestScore = CurrentScore;
        }
    }
}