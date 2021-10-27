using System;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace BricksBreaker {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour {
        [SerializeField] float speed = 10f;
        [SerializeField] Vector2 _initialDirection = Vector2.one;
        
        Rigidbody2D _rigidbody;
        Vector2 _currentVelocity;
        
        void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate() {
            if (Math.Abs(_rigidbody.velocity.sqrMagnitude - speed * speed) > Mathf.Epsilon) {
                _rigidbody.velocity = _rigidbody.velocity.normalized * speed;
            }
        }

        public void ResetVelocity() => SetVelocity(_initialDirection.normalized * speed);

        void OnCollisionEnter2D(Collision2D other) {
            foreach (var contact in other.contacts) {
                if (other.gameObject.TryGetComponent<Bricks>(out var bricks)) {
                    bricks.Hit(contact.point - other.GetContact(0).normal / 10f);
                }
            }
        }

        void SetVelocity(Vector2 newVelocity) {
            _currentVelocity = newVelocity;
            _rigidbody.velocity = _currentVelocity;
        }
    }
}
