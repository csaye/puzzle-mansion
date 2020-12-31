using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class WeakBlock : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D weakBlockRb = null;

        private void OnTriggerEnter2D(Collider2D col)
        {
            // If dynamic rigidbody found
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // Reset y velocity
                weakBlockRb.bodyType = RigidbodyType2D.Dynamic; // Fall
            }
        }
    }
}
