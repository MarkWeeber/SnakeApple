using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static SnakeApple.Space.Controls;

namespace SnakeApple.Space
{
    [CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public event Action<Vector2> OnMoveEvent;

        public float TouchDeadZoneMagnitude = 2f;
        public float TouchMaxMagnitude = 100f;

        private float touchMagnitude = 0f;
        private Controls controls;
        private bool touchStarted;
        private Vector2 touchStartPosition;
        private Vector2 touchPosition;
        private Vector2 touchDirection;

        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new Controls();
                controls.Player.SetCallbacks(this);
            }
            controls.Player.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        public void OnTouchEnter(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                touchStarted = true;
            }
            if (context.canceled)
            {
                OnMoveEvent?.Invoke(Vector2.zero);
            }
        }

        public void OnTouchPosition(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                OnMoveEvent?.Invoke(Vector2.zero);
                return;
            }
            if (touchStarted)
            {
                touchStartPosition = context.ReadValue<Vector2>();
                touchStarted = false;
            }
            touchPosition = context.ReadValue<Vector2>();
            touchMagnitude = Vector2.Distance(touchStartPosition, touchPosition);
            if (touchMagnitude > TouchDeadZoneMagnitude)
            {
                touchDirection = Vector2.ClampMagnitude(touchPosition - touchStartPosition, TouchMaxMagnitude) / TouchMaxMagnitude;
                OnMoveEvent?.Invoke(touchDirection);
            }
        }

        public void OnMoveDirection(InputAction.CallbackContext context)
        {
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }
}