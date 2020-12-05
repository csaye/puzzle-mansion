using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class Door : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Door outputDoor = null;

        public Vector2 OutputPosition
        {
            get { return (Vector2)outputDoor.transform.position + new Vector2(0.5f, 0); }
        }
    }
}
