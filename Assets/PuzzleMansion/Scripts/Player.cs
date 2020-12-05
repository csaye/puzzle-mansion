using System.Collections.Generic;
using PuzzleMansion.Objects;
using PuzzleMansion.UI;
using System.Collections;
using UnityEngine;
// using UnityEngine.SceneManagement;

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
        [SerializeField] private Transform doorPoint = null, holdPoint = null;
        [SerializeField] private Collider2D groundCheck = null;
        [SerializeField] private Fade fade = null;

        #region KeyCodes

        private const KeyCode jumpKey = KeyCode.C;
        private const KeyCode holdKey = KeyCode.X;

        private const KeyCode upKey = KeyCode.UpArrow;
        private const KeyCode downKey = KeyCode.DownArrow;
        private const KeyCode rightKey = KeyCode.RightArrow;
        private const KeyCode leftKey = KeyCode.LeftArrow;

        #endregion

        Vector2 currentVelocity = new Vector2();

        // Whether player is currently on the ground
        public bool Grounded
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
            // if (Input.GetKeyDown("p")) SceneManager.LoadScene("Chapter1");

            // Return if paused
            if (PauseManager.Paused) return;

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
            if (Input.GetKeyDown(jumpKey) && Grounded)
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
            if (Input.GetKey(upKey) && Grounded)
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
                        fade.StartFade(doorComponent.OutputPosition);
                        break;
                    }
                }
            }
        }

        // Attempts to pick up and hold block
        private void Hold()
        {
            // If hold key pressed
            if (Input.GetKeyDown(holdKey))
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
            blockRigidbody.velocity = Vector2.zero;
            blockCollider.enabled = true;
        }
    }
}
