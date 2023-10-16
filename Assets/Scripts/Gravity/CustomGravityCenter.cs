using System.Collections;
using System.Collections.Generic;
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
            CustomGravity[] customGravities = FindObjectsOfType<CustomGravity>().ToArray();
            foreach (CustomGravity item in customGravities)
            {
                item.GravityCenter = transform;
            }
        }
    }
}