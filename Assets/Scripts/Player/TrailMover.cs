using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeApple.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class TrailMover : MonoBehaviour
    {
        [SerializeField] private float maxDistanceToTarget = 1f;
        [SerializeField] private GameObject trailBodyPrefab;
        [SerializeField] private int maxTrailBodies = 50;
        [SerializeField] private Transform tail;
        [SerializeField] private List<TrailingBody> trailingBodies;
        [SerializeField] private float trailUpdateDuration = 0.05f;
        [SerializeField] private float lerpRate = 10f;

        private GameObject spawnedObject;
        private Rigidbody rb;
        private float moveDelta;
        private float trailUpdateTimer;
        private TransformData[] transformDatas;
        private bool transformsCollected;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            transformDatas = new TransformData[maxTrailBodies + 1];
            for (int i = 0; i < transformDatas.Length; i++)
            {
                transformDatas[i].position = Vector3.zero;
                transformDatas[i].rotation = Quaternion.identity;
            }
            if (trailBodyPrefab == null)
            {
                trailingBodies = new List<TrailingBody>();
            }
        }

        public void CollectibleGrabbed()
        {
            if (trailingBodies.Count < maxTrailBodies || !trailingBodies.Any())
            {
                spawnedObject = Instantiate(trailBodyPrefab);
                if (trailingBodies.Count > 0)
                {
                    spawnedObject.transform.position =
                        trailingBodies[trailingBodies.Count - 1].transform.position -
                        trailingBodies[trailingBodies.Count - 1].transform.forward * maxDistanceToTarget;
                }
                else
                {
                    spawnedObject.transform.position = tail.position - tail.forward * maxDistanceToTarget;
                }
                if (spawnedObject.TryGetComponent<TrailingBody>(out TrailingBody trailingBody))
                {
                    trailingBodies.Add(trailingBody);
                }
            }
        }

        private void Update()
        {
            StoreTransforms();
            LerpSetTransforms();
        }

        private void StoreTransforms()
        {
            trailUpdateTimer -= Time.deltaTime;
            if (trailUpdateTimer < 0)
            {
                for (int i = trailingBodies.Count; i > 0; i--)
                {
                    transformDatas[i] = transformDatas[i - 1];
                }
                transformDatas[0] = new TransformData { position = tail.position, rotation = tail.rotation };
                trailUpdateTimer = trailUpdateDuration;
                transformsCollected = true;
            }
        }

        private void LerpSetTransforms()
        {
            if (transformsCollected)
            {
                for (int i = 0; i < trailingBodies.Count; i++)
                {
                    trailingBodies[i].transform.position = Vector3.Lerp(trailingBodies[i].transform.position, transformDatas[i].position, Time.deltaTime * lerpRate);
                    trailingBodies[i].transform.rotation = Quaternion.Lerp(trailingBodies[i].transform.rotation, transformDatas[i].rotation, Time.deltaTime * lerpRate);
                }
            }
        }
    }

    [System.Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
    }
}