using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class Door : MonoBehaviour
    {
        public void OpenDoor()
        {
            Debug.Log($"Opening door at {transform.position}");
        }
    }
}
