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
        [SerializeField] private Transform spriteTransform = null;
        [SerializeField] private Transform doorPoint = null;

        Vector2 currentVelocity = new Vector2();

        // Whether player is currently on the ground
        private bool Grounded
        {
            get { return Mathf.Abs(rb.velocity.y) < Operation.epsilon; }
        }

        // Whether player is facing right
        private bool _facingRight = true;
        private bool FacingRight
        {
            get { return _facingRight; }
            set
            {
                // Return if value same, otherwise set new value
                if (value == _facingRight) return;
                _facingRight = value;

                // Flip sprite to match direction
                int xScale = FacingRight ? 1 : -1;
                spriteTransform.localScale = new Vector3(xScale, 1, 1);
            }
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

            // Set facing direction
            if (xDirection == 1) FacingRight = true;
            else if (xDirection == -1) FacingRight = false;

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
                        transform.position = doorComponent.OutputPosition;
                        break;
                    }
                }
            }
        }
    }
}
