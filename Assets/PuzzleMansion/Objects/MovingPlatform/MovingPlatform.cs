using System.Collections;
using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Vector2[] positions = null;
        [SerializeField] private float speed = 0;

        private void Start()
        {
            StartCoroutine(Move(0));
            // GetComponent<Rigidbody2D>().velocity = new Vector2(0.4f, 0);
        }

        private IEnumerator Move(int positionIndex)
        {
            // Get current position and target position
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = positions[positionIndex];

            // Get distance and time to target position
            float dist = Vector2.Distance(currentPosition, targetPosition);
            float time = dist * speed;

            // Move platform and wait
            LeanTween.move(gameObject, targetPosition, time);
            yield return new WaitForSeconds(time);

            // Get next position index and move
            int nextPositionIndex = positionIndex == positions.Length - 1 ? 0 : positionIndex + 1;
            StartCoroutine(Move(nextPositionIndex));
        }
    }
}
