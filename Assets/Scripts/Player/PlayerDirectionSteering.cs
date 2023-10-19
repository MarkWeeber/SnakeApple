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
        [SerializeField] private float rotateLerpRate = 10f;

        private Vector3 inputVector;
        private Vector3 lerpInputVector;
        private Vector3 cameraVector;
        private Camera mainCamera;
        private float yInputRotation;

        private void Start()
        {
            inputReader.OnMoveEvent += HandleMoveInput;
            mainCamera = Camera.main;
            lerpInputVector = Vector3.left;
            inputVector = Vector3.left;
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
                LerRotateDirection();
            }
        }

        private void HandleDirectionSteering()
        {
            cameraVector = mainCamera.transform.rotation * lerpInputVector;
            yInputRotation = Vector3.SignedAngle(transform.forward, cameraVector, transform.up);
            transform.Rotate(0f, yInputRotation, 0f);
        }

        private void LerRotateDirection()
        {
            lerpInputVector = Vector3.Lerp(lerpInputVector, inputVector, rotateLerpRate * Time.deltaTime);
        }

        private void HandleMoveInput(Vector2 inputSystemDirectionVector)
        {
            if (inputSystemDirectionVector.magnitude > 0.05f)
            {
                inputVector = inputSystemDirectionVector.normalized;

            }
        }
    }
}