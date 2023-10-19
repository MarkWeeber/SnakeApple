using System.Linq;
using UnityEngine;

namespace SnakeApple.Space
{
    public class CustomGravityCenter : MonoBehaviour
    {
        private void Start()
        {
            AssingGravityCenter();
        }

        private void AssingGravityCenter()
        {
            CustomGravityMovement[] customGravities = FindObjectsOfType<CustomGravityMovement>().ToArray();
            foreach (CustomGravityMovement item in customGravities)
            {
                item.GravityCenter = transform;
            }
        }
    }
}