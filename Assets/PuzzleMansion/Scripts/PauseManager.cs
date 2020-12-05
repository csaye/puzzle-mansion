using UnityEngine;

namespace PuzzleMansion
{
    public class PauseManager : MonoBehaviour
    {
        private static bool _paused = false;
        public static bool Paused
        {
            get { return _paused; }
            set
            {
                // Return if value same, otherwise set new value
                if (value == _paused) return;
                _paused = value;
                
                // Change time scale based on paused status
                Time.timeScale = Paused ? 0 : 1;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Paused = !Paused;
        }
    }
}
