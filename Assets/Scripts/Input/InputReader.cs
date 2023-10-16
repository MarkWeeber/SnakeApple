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
        public event Action<bool> OnTouch;

        public event Action<Vector2> OnTouchMove;

        private Controls controls;

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

        public void OnTouchAction(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnTouch?.Invoke(true);
            }
            else if (context.canceled)
            {
                OnTouch?.Invoke(false);
            }
        }

        public void OnTouchPosition(InputAction.CallbackContext context)
        {
            //OnTouchMove?.Invoke(context.ReadValue<Vector2>());
        }
    }
}