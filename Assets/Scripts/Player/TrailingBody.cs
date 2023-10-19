using UnityEngine;

namespace SnakeApple.Space
{
    public class TrailingBody : MonoBehaviour
    {
        [SerializeField] private Transform tail;
        public Transform Tail { get => tail; }
    }
}