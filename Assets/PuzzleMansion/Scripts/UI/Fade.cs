using PuzzleMansion.Helper;
using UnityEngine;

namespace PuzzleMansion.UI
{
    public class Fade : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Transform playerTransform;

        private Vector2 nextPosition;

        // Starts fade to move player to given position
        public void StartFade(Vector2 position)
        {
            // Set next position
            nextPosition = position;

            // Pause and start fade animation
            PauseManager.Paused = true;
            animator.SetTrigger(AnimatorHash.Fade);
        }

        // Moves player to next position
        public void MovePlayer()
        {
            playerTransform.position = nextPosition;
        }

        // Called when animation completes
        public void EndFade()
        {
            // Unpause
            PauseManager.Paused = false;
        }
    }
}
