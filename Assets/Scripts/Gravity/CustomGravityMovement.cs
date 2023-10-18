using System.Collections.Generic;
using UnityEngine;

namespace SnakeApple.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class CustomGravityMovement : MonoBehaviour
    {
        [SerializeField] private float downVelocity = 10f;
        [SerializeField] private float movementSpeed = 300f;
        [SerializeField] private float standingRotationLerpSpeed = 2f;
        [SerializeField] private float downLookingRayDistance = 2f;
        [SerializeField] private List<Transform> raycastOrigins;
        [SerializeField] private LayerMask targetMask;

        private Transform gravityCenter;
        public Transform GravityCenter { get => gravityCenter; set => gravityCenter = value; }

        private bool touchingSurface;
        private Rigidbody rb;
        private Vector3 contactPointNormal;
        private Vector3 contactPostion;
        private Vector3 forwardMovementVector;
        private Vector3 customGravitiyVector;
        private RaycastHit hit;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void Update()
        {
            CalculateIfOnSurface();
            SetPositionioning();
            //Debug.DrawRay(transform.position, contactPointNormal * 7f, Color.red);
        }

        private void FixedUpdate()
        {
            CalculateCustomGravityVector();
            CalculateForwardMovementVector();
            SetVelocity();
        }

        private void SetVelocity()
        {
            rb.velocity = Vector3.zero;
            if(!touchingSurface)
            {
                rb.velocity += customGravitiyVector * downVelocity * Time.fixedDeltaTime;
            }
            rb.velocity += forwardMovementVector;
        }

        private void CalculateCustomGravityVector()
        {
            customGravitiyVector = (transform.position - gravityCenter.position) * -1f;
        }

        private void CalculateForwardMovementVector()
        {
            if (touchingSurface)
            {
                forwardMovementVector = transform.forward * movementSpeed * Time.fixedDeltaTime;
            }
            else
            {
                forwardMovementVector = Vector3.zero;
            }
        }

        private void CalculateIfOnSurface()
        {
            touchingSurface = false;
            for (int i = 0; i < raycastOrigins.Count; i++)
            {
                if (i > 0 && !touchingSurface)
                {
                    break;
                }
                Ray ray = new Ray(raycastOrigins[i].position, raycastOrigins[i].forward);
                //Debug.DrawRay(raycastOrigins[i].position, raycastOrigins[i].forward * downLookingRayDistance, Color.cyan);
                if (Physics.Raycast(ray, out hit, downLookingRayDistance, targetMask))
                {
                    //Debug.DrawRay(hit.point, hit.normal * 1f, Color.magenta);
                    if (i == 0)
                    {
                        touchingSurface = true;
                        contactPostion = hit.point;
                        contactPointNormal = hit.normal;
                    }
                    else
                    {
                        contactPointNormal += hit.normal;
                    }
                }
            }
            contactPointNormal = contactPointNormal / raycastOrigins.Count;
        }

        private void SetPositionioning()
        {
            if(touchingSurface)
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, contactPointNormal) * transform.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, standingRotationLerpSpeed * Time.deltaTime);
                transform.position = contactPostion;
                
            }
        }
    }
}