using UnityEngine;

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

        public Vector3 newPosition;

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
            //newPosition = transform.forward * moveSpeed * Time.fixedDeltaTime;
            //transform.position += newPosition; 
            //rb.velocity += addedVelocity;
            //transform.Translate(transform.position + addedVelocity);
            
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