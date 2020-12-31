using UnityEngine;

namespace PuzzleMansion
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private bool restrictView = false;
        [SerializeField] private Vector2 minView = new Vector2();
        [SerializeField] private Vector2 maxView = new Vector2();

        [Header("References")]
        [SerializeField] private Camera playerCamera = null;
        [SerializeField] private Transform cameraFocusTransform = null;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            // Get focus position
            Vector2 focusPosition = cameraFocusTransform.position;

            // If not restricting view, set position and return
            if (!restrictView)
            {
                transform.position = new Vector3(focusPosition.x, focusPosition.y, transform.position.z);
                return;
            }

            // Get minimum and maximum world points on screen
            Vector2 screenMin = playerCamera.ScreenToWorldPoint(new Vector2(0, 0));
            Vector2 screenMax = playerCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            // Get minimum and maximum camera positions
            float minX = transform.position.x + (minView.x - screenMin.x);
            float maxX = transform.position.x + (maxView.x - screenMax.x);
            float minY = transform.position.y + (minView.y - screenMin.y);
            float maxY = transform.position.y + (maxView.y - screenMax.y);

            // Clamp camera view to be within min and max range
            float x = Mathf.Clamp(focusPosition.x, minX, maxX);
            float y = Mathf.Clamp(focusPosition.y, minY, maxY);

            // If camera out of bounds, set to center
            if (screenMax.x - screenMin.x > maxView.x - minView.x) x = 0;
            if (screenMax.y - screenMin.y > maxView.y - minView.y) y = 0;

            // Move camera to position
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
