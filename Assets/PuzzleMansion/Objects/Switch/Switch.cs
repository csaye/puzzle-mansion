using System;
using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class Switch : MonoBehaviour
    {
        public static event Action onSwitch;

        private void OnTriggerEnter2D(Collider2D col)
        {
            // If dynamic rigidbody found
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // Reset y velocity
                onSwitch?.Invoke(); // Trigger switch
            }
        }
    }
}
