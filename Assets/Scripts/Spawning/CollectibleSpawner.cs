using UnityEngine;

namespace SnakeApple.Space
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [SerializeField] private Transform gravityCenter;
        [SerializeField] private GameObject collectiblePrefab;
        [SerializeField] private int spawnQuantity = 50;
        [Tooltip("The Sphere cast radius must cover whole mesh collider!")]
        [SerializeField] private float sphereCastRadius = 30f;
        [SerializeField] private float standingHeight = 0.3f;

        private int layerMask;

        private void Start()
        {
            layerMask = (1 << gravityCenter.gameObject.layer);
            SpawnPrefabs();
        }

        private void SpawnPrefabs()
        {
            for (int i = 0; i < spawnQuantity; i++)
            {
                GameObject spawnedPrefab = Instantiate(collectiblePrefab);
                spawnedPrefab.transform.position = GetRandomSpawnPosition();
                if (spawnedPrefab.TryGetComponent<Collectible>(out Collectible collectible))
                {
                    collectible.CollectibleSpawner = this;
                }

            }
        }

        public Vector3 GetRandomSpawnPosition()
        {
            Vector3 spawnPosition = Vector3.zero;
            Vector3 randomSpherePoint = Random.insideUnitSphere;
            randomSpherePoint = randomSpherePoint.normalized;
            Vector3 raycastPoint = gravityCenter.position + randomSpherePoint * sphereCastRadius;
            Vector3 raycastDirection = gravityCenter.position - raycastPoint;
            Ray ray = new Ray(raycastPoint, raycastDirection.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, sphereCastRadius, layerMask))
            {
                spawnPosition = hit.point + hit.normal * standingHeight;
            }
            return spawnPosition;
        }

    }
}