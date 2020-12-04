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

        Vector2 currentVelocity = new Vector2();

        private bool Grounded
        {
            get { return Mathf.Abs(rb.velocity.y) < Operation.epsilon; }
        }

        private void Update()
        {
            Jump();
            Move();
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Grounded)
            {
                rb.AddForce(new Vector2(0, jumpForce));
            }
        }

        private void Move()
        {
            int xDirection = 0;
            if (Input.GetKey("a")) xDirection -= 1;
            if (Input.GetKey("d")) xDirection += 1;
            Vector2 targetVelocity = new Vector2(xDirection * movementForce, rb.velocity.y);
    
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime, maxSpeed);
        }
    }
}
