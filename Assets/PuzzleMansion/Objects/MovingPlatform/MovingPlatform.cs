using System.Collections;
using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private bool loop = false;
        [SerializeField] private Vector2[] positions = null;
        [SerializeField] [Range(1, 10)] private float speed = 1;

        [Header("References")]
        [SerializeField] private Rigidbody2D rb = null;

        private bool started = false;

        private void OnTriggerEnter2D(Collider2D col)
        {
            // If grounded player
            if (!started && col.CompareTag("Player") && Player.instance.grounded)
            {
                started = true;
                StartCoroutine(Move(0));
            }
        }

        private IEnumerator Move(int positionIndex)
        {
            // Get current position and target position
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = (Vector2)transform.position + positions[positionIndex];

            // Get distance and time to target position
            float dist = Vector2.Distance(currentPosition, targetPosition);
            float time = dist / speed;

            // Get necessary velocity
            Vector2 velocity = new Vector2(targetPosition.x - currentPosition.x, targetPosition.y - currentPosition.y);
            velocity = velocity.normalized * speed;

            // Set velocity and wait
            rb.velocity = velocity;
            yield return new WaitForSeconds(time);
            rb.velocity = Vector2.zero;

            // Get next position index and move
            int nextPositionIndex = positionIndex == positions.Length - 1 ? 0 : positionIndex + 1;
            if (loop || nextPositionIndex != 0) StartCoroutine(Move(nextPositionIndex));
        }
    }
}
