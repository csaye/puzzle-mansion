using UnityEngine;

namespace PuzzleMansion
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float lerpTime = 0;
        // [SerializeField] private bool restrictView = false;
        // [SerializeField] private Vector2 minView = new Vector2();
        // [SerializeField] private Vector2 maxView = new Vector2();

        // [Header("References")]
        // [SerializeField] private Camera playerCamera = null;
        // [SerializeField] private Transform cameraFocusTransform = null;

        // private void Update()
        // {
        //     Move();
        // }

        // private Coroutine co;
        // private int tweenId;

        public static PlayerCamera instance;

        private void Awake()
        {
            instance = this;
        }

        public void Focus(Vector2 focusPosition)
        {
            LeanTween.move(gameObject, focusPosition, lerpTime);
        }

        // private void Move()
        // {
        //     // Get focus position
        //     Vector2 focusPosition = cameraFocusTransform.position;

        //     // If not restricting view, set position and return
        //     if (!restrictView)
        //     {
        //         transform.position = new Vector3(focusPosition.x, focusPosition.y, transform.position.z);
        //         return;
        //     }

        //     // Get minimum and maximum world points on screen
        //     Vector2 screenMin = playerCamera.ScreenToWorldPoint(new Vector2(0, 0));
        //     Vector2 screenMax = playerCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        //     // Get minimum and maximum camera positions
        //     float minX = transform.position.x + (minView.x - screenMin.x);
        //     float maxX = transform.position.x + (maxView.x - screenMax.x);
        //     float minY = transform.position.y + (minView.y - screenMin.y);
        //     float maxY = transform.position.y + (maxView.y - screenMax.y);

        //     // Clamp camera view to be within min and max range
        //     float x, y;
        //     if (minX > maxX) x = focusPosition.x; else x = Mathf.Clamp(focusPosition.x, minX, maxX);
        //     if (minY > maxY) y = focusPosition.y; else y = Mathf.Clamp(focusPosition.y, minY, maxY);

        //     // Move camera to position
        //     transform.position = new Vector3(x, y, transform.position.z);
        // }
    }
}
