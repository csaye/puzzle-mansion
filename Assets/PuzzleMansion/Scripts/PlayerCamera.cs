using UnityEngine;

namespace PuzzleMansion
{
    [System.Serializable]
    public struct CameraView
    {
        public Vector2 min;
        public Vector2 max;
    }

    public class PlayerCamera : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private CameraView cameraView = new CameraView();

        [Header("References")]
        [SerializeField] private Camera playerCamera = null;
        [SerializeField] private Transform cameraFocusTransform = null;

        public static PlayerCamera instance;

        private void Awake()
        {
            instance = this;
        }

        public void SetCameraView(CameraView _cameraView)
        {
            cameraView = _cameraView;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            // Get focus position
            Vector2 focusPosition = cameraFocusTransform.position;

            // Get minimum and maximum world points on screen
            Vector2 screenMin = playerCamera.ScreenToWorldPoint(new Vector2(0, 0));
            Vector2 screenMax = playerCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            // Get minimum and maximum camera positions
            float minX = transform.position.x + (cameraView.min.x - screenMin.x);
            float maxX = transform.position.x + (cameraView.max.x - screenMax.x);
            float minY = transform.position.y + (cameraView.min.y - screenMin.y);
            float maxY = transform.position.y + (cameraView.max.y - screenMax.y);

            // Clamp camera view to be within min and max range
            float x = Mathf.Clamp(focusPosition.x, minX, maxX);
            float y = Mathf.Clamp(focusPosition.y, minY, maxY);

            // If camera out of bounds, set to center
            Vector2 midpoint = (cameraView.max - cameraView.min) / 2;
            if (screenMax.x - screenMin.x > cameraView.max.x - cameraView.min.x) x = midpoint.x;
            if (screenMax.y - screenMin.y > cameraView.max.y - cameraView.min.y) y = midpoint.y;

            // Move camera to position
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
