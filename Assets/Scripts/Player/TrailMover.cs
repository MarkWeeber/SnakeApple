using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
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

        private GameObject spawnedObject;
        private Rigidbody rb;
        private float moveDelta;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (trailBodyPrefab == null)
            {
                trailingBodies = new List<TrailingBody>();
            }
        }

        public void CollectibleGrabbed()
        {
            if (trailingBodies.Count <= maxTrailBodies || !trailingBodies.Any())
            {
                spawnedObject = Instantiate(trailBodyPrefab, transform.position, Quaternion.identity);
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
            ManageMovementParralelJob();
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

    [BurstCompatible]
    public struct TrailBodyMoveJobParallelFor : IJobParallelFor
    {
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
            if (currentDistance > maxDistance + 0.1f)
            {
                currentPositions[index] = previousPositions[index] - direction.normalized * maxDistance;
            }
            else
            {
                currentPositions[index] += currentRotations[index] * Vector3.forward * moveDelta;
            }    
        }
    }
}