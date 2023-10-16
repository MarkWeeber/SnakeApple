using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnakeApple.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotateSpeed = 30f;

        private Rigidbody rb;
        private bool alive;
        private float steerValue;

        private void Start()
        {
            alive = true;
            rb = GetComponent<Rigidbody>();
            inputReader.OnMoveEvent += HandleSteering;
        }

        private void OnDestroy()
        {
            inputReader.OnMoveEvent -= HandleSteering;
        }

        private void FixedUpdate()
        {
            if(alive)
            {
                MoveForward();
                Steer();
            }
        }

        private void MoveForward()
        {
            //rb.velocity = transform.forward * moveSpeed * Time.fixedDeltaTime;
            
        }

        private void Steer()
        {
            transform.Rotate(0f, steerValue * rotateSpeed * Time.fixedDeltaTime, 0f, UnityEngine.Space.Self);
        }

        private void HandleSteering(Vector2 vector)
        {
            steerValue = vector.x;
        }
    }
}