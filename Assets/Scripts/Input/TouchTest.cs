using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace SnakeApple.Space
{
    public class TouchTest : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Vector2 moveDirection;

        private void Start()
        {
            inputReader.OnMoveEvent += HandleOnMoveEvent;
        }

        private void OnDestroy()
        {
            inputReader.OnMoveEvent -= HandleOnMoveEvent;
        }

        private void HandleOnMoveEvent(Vector2 moveDirection)
        {
            this.moveDirection = moveDirection;
        }
    }
}