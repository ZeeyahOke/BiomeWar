using UnityEngine;

namespace BiomeWar
{
    // Keeps the object sitting on the terrain, including while it moves.
    public class GroundSnap : MonoBehaviour
    {
        [SerializeField] float fallSpeed = 15f;
        [SerializeField] float rayHeight = 3f;
        [SerializeField] float rayLength = 20f;
        [SerializeField] float groundOffset = 0f;
        [SerializeField] LayerMask groundMask = 1;

        void LateUpdate()
        {
            Vector3 origin = transform.position + Vector3.up * rayHeight;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength, groundMask))
            {
                Vector3 pos = transform.position;
                float targetY = hit.point.y + groundOffset;

                // Snap down instantly, ease down when falling from a height.
                pos.y = pos.y > targetY + 0.5f
                    ? Mathf.MoveTowards(pos.y, targetY, fallSpeed * Time.deltaTime)
                    : targetY;

                transform.position = pos;
            }
            else
            {
                transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            }
        }
    }
}
