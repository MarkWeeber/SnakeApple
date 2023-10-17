using UnityEngine;

namespace SnakeApple.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class CustomGravity : MonoBehaviour
    {
        [SerializeField] private float gravityScale = 9.8f;
        [SerializeField] private bool rotateStandingOnSurface = false;
        [SerializeField] private float standingRotationSpeed = 10f;

        public float moveSpeed = 5f;

        private Transform gravityCenter;
        public Transform GravityCenter { get => gravityCenter; set => gravityCenter = value; }

        private Rigidbody rb;
        private Vector3 contactPointNormal;
        private bool touchingSurface;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            HandleCustomGravity();
        }

        private void HandleCustomGravity()
        {
            if (gravityCenter != null || !rb.useGravity)
            {
                Vector3 direction = transform.position - gravityCenter.position;
                rb.AddForce(-direction.normalized * gravityScale * rb.mass);
                if (rotateStandingOnSurface)
                {
                    HandleStandingOnSurfaceRotation();
                }
            }
        }

        private void HandleStandingOnSurfaceRotation()
        {
            if(touchingSurface)
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, contactPointNormal) * transform.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, standingRotationSpeed * Time.fixedDeltaTime);
                Vector3 newPosition = transform.forward * moveSpeed * Time.fixedDeltaTime;
                rb.velocity = Vector3.ClampMagnitude(rb.velocity + newPosition, 10f);
                
                //transform.position += newPosition;
            }

        }

        private void OnCollisionStay(Collision collision)
        {
            if(rotateStandingOnSurface)
            {
                touchingSurface = false;
                if (collision.transform == gravityCenter)
                {
                    ContactPoint contactPoint = collision.contacts[0];
                    contactPointNormal = contactPoint.normal;
                    touchingSurface = true;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.transform == gravityCenter)
            {
                touchingSurface = false;
            }
        }
    }
}