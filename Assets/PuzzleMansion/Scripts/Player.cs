using PuzzleMansion.Objects;
using UnityEngine;

namespace PuzzleMansion
{
    public class Player : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] [Range(0, 1)] private float smoothTime = 0;
        [SerializeField] [Range(0, 100)] private int maxSpeed = 0;
        [SerializeField] [Range(0, 1000)] private int jumpForce = 0;
        [SerializeField] [Range(0, 50)] private int movementForce = 0;

        [Header("References")]
        [SerializeField] private Rigidbody2D rb = null;
        [SerializeField] private Transform doorPoint = null;

        Vector2 currentVelocity = new Vector2();

        // Whether player is currently on the ground
        private bool Grounded
        {
            get { return Mathf.Abs(rb.velocity.y) < Operation.epsilon; }
        }

        private void Update()
        {
            Jump();
            Move();
            Up();
        }

        // Jumps
        private void Jump()
        {
            // If jump key pressed and player grounded
            if (Input.GetKeyDown(KeyCode.Space) && Grounded)
            {
                // Add jump force
                rb.AddForce(new Vector2(0, jumpForce));
            }
        }

        // Moves player left and right
        private void Move()
        {
            // Get x direction from left and right keys
            int xDirection = 0;
            if (Input.GetKey(KeyCode.A)) xDirection -= 1;
            if (Input.GetKey(KeyCode.D)) xDirection += 1;

            // Get target velocity from x direction
            Vector2 targetVelocity = new Vector2(xDirection * movementForce, rb.velocity.y);
    
            // Smooth damp velocity based on target velocity
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime, maxSpeed);
        }

        // Checks door entry on up key press
        private void Up()
        {
            // If up key pressed and player grounded
            if (Input.GetKeyDown(KeyCode.W) && Grounded)
            {
                // Check for door
                Collider2D[] results = Physics2D.OverlapPointAll(doorPoint.position);
                foreach (Collider2D hitCol in results)
                {
                    // If found self, continue
                    if (hitCol.gameObject == gameObject) continue;

                    // If door component found, open door and break
                    Door doorComponent = hitCol.gameObject.GetComponent<Door>();
                    if (doorComponent != null)
                    {
                        doorComponent.OpenDoor(this);
                        break;
                    }
                }
            }
        }
    }
}
