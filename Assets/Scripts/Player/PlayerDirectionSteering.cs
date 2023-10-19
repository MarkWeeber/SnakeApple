using UnityEngine;

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
        private float maxRotation;

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

        private void LateUpdate()
        {
            LerRotateDirection();
            HandleDirectionSteering();
        }

        private void HandleDirectionSteering()
        {
            cameraVector = mainCamera.transform.rotation * inputVector;
            yInputRotation = Vector3.SignedAngle(transform.forward, cameraVector, transform.up);
            if (Mathf.Abs(yInputRotation) < maxRotation)
            {
                transform.Rotate(0f, yInputRotation, 0f);
                maxRotation = Mathf.Abs(yInputRotation);
            }
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
                maxRotation = 355f;
            }
        }
    }
}