using System;

using UnityEngine;

namespace BricksBreaker {
    [RequireComponent(typeof(Collider2D))]
    public class DeathZone : MonoBehaviour {
        public event Action BallEntered;
        
        void Awake() {
            var collider2d = GetComponent<Collider2D>();
            collider2d.isTrigger = true;
        }

        void OnTriggerEnter2D(Collider2D other) {
            var ball = other.GetComponentInParent<Ball>();
            if (ball != null) BallEntered?.Invoke();
        }
    }
}