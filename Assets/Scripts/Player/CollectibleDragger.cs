using System.Collections.Generic;
using UnityEngine;

namespace SnakeApple.Space
{
    public class CollectibleDragger : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float dragInSpeed = 10f;

        private List<Transform> collectiblesWithinRange;

        private void Start()
        {
            collectiblesWithinRange = new List<Transform>();
        }

        private void Update()
        {
            ManageCollectibleDragging();
        }

        private void ManageCollectibleDragging()
        {
            foreach (Transform collectible in collectiblesWithinRange)
            {
                Vector3 direction = transform.position - collectible.position;
                direction = direction.normalized;
                collectible.position += direction * dragInSpeed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Utils.CheckLayer(targetMask, other.gameObject.layer))
            {
                collectiblesWithinRange.Add(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Utils.CheckLayer(targetMask, other.gameObject.layer))
            {
                collectiblesWithinRange.Remove(other.transform);
            }
        }
    }
}