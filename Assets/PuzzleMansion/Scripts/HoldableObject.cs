using UnityEngine;

namespace PuzzleMansion
{
    public class HoldableObject : MonoBehaviour
    {
        [Header("References")]
        public Collider2D col;
        public Rigidbody2D rb;
    }
}
