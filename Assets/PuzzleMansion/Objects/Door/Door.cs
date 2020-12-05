using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Door outputDoor = null;

        public void OpenDoor(Player player)
        {
            player.transform.position = outputDoor.transform.position;
        }
    }
}
