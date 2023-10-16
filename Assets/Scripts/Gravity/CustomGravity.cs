using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEditor.Progress;

namespace SnakeApple.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class CustomGravity : MonoBehaviour
    {
        [SerializeField] private float gravityScale = 9.8f;
        [SerializeField] private bool rotateStandingOnSurface = false;
        [SerializeField] private float standingRotationSpeed = 10f;

        private Transform gravityCenter;
        public Transform GravityCenter { get => gravityCenter; set => gravityCenter = value; }

        private Rigidbody rb;
        private Vector3 contactPointNormal;
        private Quaternion contactPointRotation;
        private Vector3 lookRotation;

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
            transform.up = Vector3.Slerp(transform.up, contactPointNormal, standingRotationSpeed * Time.fixedDeltaTime);
            //lookRotation = (transform.position - contactPointNormal);
            //lookRotation.y = transform.rotation.y;
            //transform.rotation = Quaternion.LookRotation(lookRotation);
            //transform.rotation = Quaternion.Euler(contactPointNormal.x, contactPointNormal.y, contactPointNormal.z);
            //transform.LookAt(contactPointNormal);
        }

        private void OnCollisionStay(Collision collision)
        {
            if(rotateStandingOnSurface)
            {
                ContactPoint contactPoint = collision.contacts[0];
                contactPointNormal = contactPoint.normal;
            }
        }
    }
}