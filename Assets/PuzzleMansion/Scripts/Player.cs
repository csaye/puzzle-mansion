using PuzzleMansion.UI;
using PuzzleMansion.Objects;
using System.Collections;
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
        [SerializeField] private Animator animator = null;
        [SerializeField] private Transform doorPoint = null, holdPoint = null;
        [SerializeField] private Collider2D groundCheck = null;

        #region KeyCodes

        private const KeyCode jumpKey = KeyCode.Space;
        private const KeyCode holdKey = KeyCode.LeftShift;

        private const KeyCode upKey = KeyCode.W;
        private const KeyCode downKey = KeyCode.S;
        private const KeyCode rightKey = KeyCode.D;
        private const KeyCode leftKey = KeyCode.A;

        #endregion

        Vector2 currentVelocity = new Vector2();

        // Whether player is currently on the ground
        public bool grounded
        {
            get
            {
                // Check for nontrigger collider in ground check collider
                Collider2D[] results = Physics2D.OverlapBoxAll(groundCheck.bounds.center, groundCheck.bounds.size, 0);
                foreach (Collider2D hitCol in results)
                {
                    // If found collider which is not self and not trigger
                    if (hitCol.gameObject != gameObject && !hitCol.isTrigger) return true;
                }
                return false;
            }
        }

        // Whether player is facing right
        private bool _facingRight = true;
        private bool facingRight
        {
            get { return _facingRight; }
            set
            {
                // Return if value same, otherwise set new value
                if (value == _facingRight) return;
                _facingRight = value;

                // Set animator
                animator.SetBool("FacingRight", facingRight);

                // Set local hold point x
                Vector3 pos = holdPoint.transform.localPosition;
                float x = facingRight ? 1 : -1;
                holdPoint.transform.localPosition = new Vector3(x, pos.y, pos.z);
            }
        }

        // Whether player is holding an object
        private bool _holding = false;
        private bool holding
        {
            get { return _holding; }
            set
            {
                // Return if value same, otherwise set new value
                if (value == _holding) return;
                _holding = value;

                // Set animator
                animator.SetBool("Holding", holding);
            }
        }

        public static Player instance;
        
        private void Awake()
        {
            if (instance == null) instance = this;
        }

        private void Update()
        {
            // Return if paused
            if (PauseManager.paused) return;

            // Act on inputs
            Jump();
            Move();
            Up();
            Hold();
        }

        // Jumps
        private void Jump()
        {
            // If jump key pressed and player grounded
            if (Input.GetKeyDown(jumpKey) && grounded)
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
            if (Input.GetKey(leftKey)) xDirection -= 1;
            if (Input.GetKey(rightKey)) xDirection += 1;

            // Set facing direction
            if (xDirection == 1) facingRight = true;
            else if (xDirection == -1) facingRight = false;

            // Get target velocity from x direction
            Vector2 targetVelocity = new Vector2(xDirection * movementForce, rb.velocity.y);
    
            // Smooth damp velocity based on target velocity
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime, maxSpeed);

            // Set animator
            animator.SetBool("Moving", Mathf.Abs(rb.velocity.magnitude) > 0.1f); 
        }

        // Checks door entry on up key press
        private void Up()
        {
            // If up key pressed and player grounded
            if (Input.GetKey(upKey) && grounded)
            {
                // Get all colliders at door point
                Collider2D[] colliders = Physics2D.OverlapPointAll(doorPoint.position);
                foreach (Collider2D hitCol in colliders)
                {
                    // If self, skip collider
                    if (hitCol.gameObject == gameObject) continue;

                    // If door component found
                    Door doorComponent = hitCol.gameObject.GetComponent<Door>();
                    if (doorComponent != null)
                    {
                        // Reset velocity and start fade to output position
                        rb.velocity = Vector2.zero;
                        Fade.instance.StartFade(doorComponent.OutputPosition);
                        break;
                    }
                }
            }
        }

        // Attempts to pick up and hold block
        private void Hold()
        {
            // If not already holding and hold key pressed
            if (!holding && Input.GetKey(holdKey))
            {
                // Get all colliders at hold point
                Collider2D[] colliders = Physics2D.OverlapPointAll(holdPoint.position);
                foreach (Collider2D hitCol in colliders)
                {
                    // If self, skip collider
                    if (hitCol.gameObject == gameObject) continue;

                    // If holdable component found
                    HoldableObject holdableComponent = hitCol.gameObject.GetComponent<HoldableObject>();
                    if (holdableComponent != null)
                    {
                        // Start holding block and break
                        StartCoroutine(HoldBlock(hitCol.gameObject.transform, holdableComponent.col, holdableComponent.rb));
                        break;
                    }
                }
            }
        }

        private IEnumerator HoldBlock(Transform blockTransform, Collider2D blockCollider, Rigidbody2D blockRigidbody)
        {
            holding = true;

            // Disable block collider
            blockCollider.enabled = false;

            // While hold key pressed
            while (Input.GetKey(holdKey))
            {
                // Wait
                yield return null;

                // Keep block at hold point
                blockTransform.position = holdPoint.position;
            }

            // Reset rigidbody velocity and collider on release
            blockRigidbody.velocity = rb.velocity;
            blockCollider.enabled = true;

            holding = false;
        }
    }
}
