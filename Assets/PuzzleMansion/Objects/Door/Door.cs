using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class Door : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Door outputDoor = null;

        public Vector2 OutputPosition
        {
            get { return outputDoor.transform.position; }
        }
    }
}
