using UnityEngine;

namespace SnakeApple.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerSteering : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private float rotateSpeed = 30f;

        private bool alive;
        private float steerValue;

        private void Start()
        {
            alive = true;
            inputReader.OnMoveEvent += HandleSteering;
        }

        private void OnDestroy()
        {
            inputReader.OnMoveEvent -= HandleSteering;
        }

        private void Update()
        {
            if (alive)
            {
                Steer();
            }
        }

        private void Steer()
        {
            transform.Rotate(0f, steerValue * rotateSpeed * Time.deltaTime, 0f, UnityEngine.Space.Self);
        }

        private void HandleSteering(Vector2 inputSystemVector)
        {
            steerValue = inputSystemVector.x;
        }
    }
}