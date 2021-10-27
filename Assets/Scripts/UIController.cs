using System;
using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace BricksBreaker
{
    public class UIController : MonoBehaviour {
        [SerializeField, Required] LevelController levelController;
        
        [SerializeField, Required] TMP_Text scoreText;
        [SerializeField, Required] TMP_Text healthText;

        [Title("GameOverScreen")]
        [SerializeField, Required] GameObject gameOverScreen;
        [SerializeField, Required] Button restartButton;
        [SerializeField, Required] Button exitButton;

        void Awake() {
            levelController.ScoreChanged += SetScore;
            SetScore(levelController.Score);
            
            levelController.HealthChanged += SetHealth;
            SetHealth(levelController.Health);

            levelController.GameOver += OnGameOver;
            gameOverScreen.SetActive(false);
            
            restartButton.onClick.AddListener(levelController.Restart);
            exitButton.onClick.AddListener(levelController.Exit);
        }

        void OnGameOver() {
            gameOverScreen.SetActive(true);
        }

        void SetScore(int score) => scoreText.text = score.ToString();

        void SetHealth(int health) => healthText.SetText(health.ToString());
    }
}
