using UnityEngine;

namespace PuzzleMansion
{
    public class Spring : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private int springForce = 0;

        private void OnTriggerEnter2D(Collider2D col)
        {
            // If dynamic rigidbody found
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // Reset y velocity
                rb.AddForce(new Vector2(0, springForce)); // Add spring force
            }
        }
    }
}
