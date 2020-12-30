using PuzzleMansion.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuzzleMansion.UI
{
    public class Fade : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;

        private Vector2 nextPosition;

        public static Fade instance;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        // Starts fade to move player to given position
        public void StartFade(Vector2 position)
        {
            // Set next position
            nextPosition = position;

            // Pause and start fade animation
            PauseManager.paused = true;
            animator.SetTrigger(AnimatorHash.Fade);
        }

        private void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads current scene
        private void MovePlayer() => Player.instance.transform.position = nextPosition; // Moves player to next position
        private void EndFade() => PauseManager.paused = false; // Unpause when animation completes
    }
}
