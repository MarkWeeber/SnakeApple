using UnityEngine;

namespace SnakeApple.Space
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask;

        private CollectibleSpawner collectibleSpawner;
        public CollectibleSpawner CollectibleSpawner { get => collectibleSpawner; set => collectibleSpawner = value; }

        private void PlaceToNewRandomPosition()
        {
            transform.position = collectibleSpawner.GetRandomSpawnPosition();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Utils.CheckLayer(targetMask, other.gameObject.layer))
            {
                if (other.gameObject.TryGetComponent<TrailMover>(out TrailMover trailMover))
                {
                    trailMover.CollectibleGrabbed();
                }
                PlaceToNewRandomPosition();
            }
        }
    }
}