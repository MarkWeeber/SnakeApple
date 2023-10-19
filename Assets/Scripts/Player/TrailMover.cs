using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

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

        private void ManageMovementParralelJob()
        {
            moveDelta = rb.velocity.magnitude;
            NativeArray<Vector3> positions = new NativeArray<Vector3>(trailingBodies.Count, Allocator.TempJob);
            NativeArray<Vector3> previousPositions = new NativeArray<Vector3>(trailingBodies.Count, Allocator.TempJob);
            NativeArray<Quaternion> rotations = new NativeArray<Quaternion>(trailingBodies.Count, Allocator.TempJob);
            for (int i = 0; i < trailingBodies.Count; i++)
            {
                positions[i] = trailingBodies[i].Tail.transform.position;
                rotations[i] = trailingBodies[i].transform.rotation;
                if (i == 0)
                {
                    previousPositions[i] = tail.position;
                    continue;
                }
                previousPositions[i] = trailingBodies[i - 1].Tail.transform.position;
            }
            TrailBodyMoveJobParallelFor trailBodyMoveJobParallelFor = new TrailBodyMoveJobParallelFor {
                deltaTime = Time.deltaTime,
                moveDelta = moveDelta,
                currentPositions = positions,
                currentRotations = rotations,
                previousPositions = previousPositions,
                maxDistance = maxDistanceToTarget
            };
            JobHandle jobHandle = trailBodyMoveJobParallelFor.Schedule(trailingBodies.Count, 10);
            jobHandle.Complete();
            for (int i = 0; i < trailingBodies.Count; i++)
            {
                trailingBodies[i].transform.position = positions[i];
                trailingBodies[i].transform.rotation = rotations[i];
            }
            positions.Dispose();
            rotations.Dispose();
            previousPositions.Dispose();
        }
    }

    [BurstCompile]
    public struct TrailBodyMoveJobParallelFor : IJobParallelFor
    {
        public float deltaTime;
        public float moveDelta;
        public NativeArray<Vector3> currentPositions;
        public NativeArray<Quaternion> currentRotations;
        public NativeArray<Vector3> previousPositions;
        public float maxDistance;
        public void Execute(int index)
        {
            Vector3 direction = previousPositions[index] - currentPositions[index];
            Quaternion rotationDirection = Quaternion.LookRotation(direction);
            currentRotations[index] = rotationDirection;
            float currentDistance = direction.magnitude;
            if (currentDistance > maxDistance)
            {
                currentPositions[index] = previousPositions[index] - direction.normalized * maxDistance;
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