using UnityEngine;

namespace PuzzleMansion
{
    public class FocusCollider : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Vector2 focusPosition = new Vector2();

        private void OnTriggerEnter2D(Collider2D col)
        {
            // If player, focus camera
            if (col.CompareTag("Player")) PlayerCamera.instance.Focus(focusPosition);
        }
    }
}
