using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SnakeApple.Space
{
    public class PlayerDirectionSteering : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;

        private Vector3 inputVector;
        private Vector3 cameraVector;
        private Camera mainCamera;
        private float yInputRotation;
        private bool calculateRotation;
        

        private void Start()
        {
            inputReader.OnMoveEvent += HandleMoveInput;
            mainCamera = Camera.main;
        }

        private void OnDestroy()
        {
            inputReader.OnMoveEvent -= HandleMoveInput;
        }

        private void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                HandleDirectionSteering();
            }
        }

        private void HandleDirectionSteering()
        {
            if (calculateRotation)
            {
                cameraVector = mainCamera.transform.rotation * inputVector;
                yInputRotation = Vector3.SignedAngle(transform.forward, cameraVector, transform.up);
                transform.Rotate(0f, yInputRotation, 0f);
                calculateRotation = false;
            }
        }

        private void HandleMoveInput(Vector2 inputSystemDirectionVector)
        {
            if (inputSystemDirectionVector.magnitude > 0.05f)
            {
                inputVector = inputSystemDirectionVector.normalized;
                calculateRotation = true;
            }
        }
    }
}