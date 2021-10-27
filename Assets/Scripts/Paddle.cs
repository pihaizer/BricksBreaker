using System;

using UnityEngine;

namespace BricksBreaker {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Paddle : MonoBehaviour {
        [SerializeField] float speed;
        
        Rigidbody2D _rigidbody;
        
        void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update() {
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            _rigidbody.velocity = new Vector2(inputHorizontal * speed, 0);
        }
    }
}