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
        [SerializeField] private Vector2 touchDelta;


        private void Start()
        {
            inputReader.OnTouch += HandleOnTouch;
            //inputReader.OnTouchMove += HandleOnTouchMove;
        }

        private void OnDestroy()
        {
            inputReader.OnTouch -= HandleOnTouch;
            //inputReader.OnTouchMove -= HandleOnTouchMove;
        }

        private void Update()
        {

        }

        private void HandleOnTouchMove(Vector2 obj)
        {
            touchDelta = obj;
        }


        private void HandleOnTouch(bool obj)
        {
            //Debug.Log(obj);
        }
    }
}