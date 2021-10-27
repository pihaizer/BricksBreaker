using System;
using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BricksBreaker
{
    public class LevelController : MonoBehaviour {
        [SerializeField, Required] Bricks _bricks;
        [SerializeField] int maxHealth = 3;
        [SerializeField, Required] Ball ball;
        [SerializeField, Required] Paddle paddle;
        [SerializeField, Required] DeathZone deathZone;

        [SerializeField] bool useRandomLayout;

        public event Action<int> ScoreChanged;
        public event Action<int> HealthChanged;
        public event Action GameOver;
        
        public int Score { get; private set; }
        public int Health { get; private set; }

        void Awake() {
            _bricks.BrickDestroyed += OnBrickDestroyed;
            deathZone.BallEntered += DeathZoneOnBallEntered;
            Health = maxHealth;
            if (useRandomLayout) _bricks.GenerateRandomLayout();
        }

        void Start() {
            ResetBallPosition();
            Time.timeScale = 1;
        }

        public void Restart() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Exit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }

        void DeathZoneOnBallEntered() {
            Debug.Log("Entered death zone");
            Health--;
            HealthChanged?.Invoke(Health);
            if (Health > 0) {
                ResetBallPosition();
            } else {
                Time.timeScale = 0;
                GameOver?.Invoke();
            }
        }

        void OnBrickDestroyed(Brick brick) {
            Score += brick.score;
            ScoreChanged?.Invoke(Score);
        }

        void ResetBallPosition() {
            ball.transform.position = paddle.transform.position + Vector3.up;
            ball.ResetVelocity();
        }
    }
}
