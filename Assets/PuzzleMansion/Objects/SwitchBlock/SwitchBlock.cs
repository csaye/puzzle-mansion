using UnityEngine;

namespace PuzzleMansion.Objects
{
    public class SwitchBlock : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private Sprite activeSprite= null, inactiveSprite = null;
        [SerializeField] private Collider2D col = null;

        // Active property which automatically updates sprite and collider
        private bool _active = false;
        private bool active
        {
            get { return _active; }
            set
            {
                _active = value;

                spriteRenderer.sprite = active ? activeSprite : inactiveSprite; // Set sprite
                col.enabled = active; // Set collider
            }
        }

        // Toggle active on switch
        private void OnSwitch()
        {
            active = !active;
        }

        private void OnEnable()
        {
            Switch.onSwitch += OnSwitch;
        }

        private void OnDisable()
        {
            Switch.onSwitch -= OnSwitch;
        }
    }
}
