using PuzzleMansion.UI;
using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class Door : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Door outputDoor = null;
        [SerializeField] private CameraView cameraView = new CameraView();

        public Vector2 outputPosition
        {
            get { return outputDoor.transform.position; }
        }

        public void EnterDoor()
        {
            // Start fade to output position and view
            Fade.instance.StartFade(outputPosition, cameraView);
        }
    }
}
