using UnityEngine;

namespace BiomeWar
{
    // Keeps the object sitting on the ground, including while it moves.
    public class GroundSnap : MonoBehaviour
    {
        [SerializeField] float fallSpeed = 15f;
        [SerializeField] float rayHeight = 3f;
        [SerializeField] float rayLength = 20f;
        [SerializeField] float groundOffset = 0f;
        [SerializeField] LayerMask groundMask = 1;

        [Header("Debug")]
        [SerializeField] bool logRaycast;

        void LateUpdate()
        {
            // Ray distances are local space, so divide by scale to keep them
            // consistent in world units across differently scaled prefabs.
            float scale = Mathf.Max(0.01f, transform.lossyScale.y);
            float height = rayHeight / scale;
            float length = rayLength / scale;

            Vector3 origin = transform.position + Vector3.up * height;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, length, groundMask))
            {
                if (logRaycast)
                    Debug.Log($"{name}: hit '{hit.collider.name}' at y={hit.point.y:F2} (self y={transform.position.y:F2})");

                Vector3 pos = transform.position;
                float targetY = hit.point.y + groundOffset;

                pos.y = pos.y > targetY + 0.5f
                    ? Mathf.MoveTowards(pos.y, targetY, fallSpeed * Time.deltaTime)
                    : targetY;

                transform.position = pos;
            }
            else
            {
                if (logRaycast)
                    Debug.Log($"{name}: ray hit NOTHING. origin y={origin.y:F2}, length={length:F2}, scale={scale:F2}");

                transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            }
        }

        void OnDrawGizmosSelected()
        {
            float scale = Mathf.Max(0.01f, transform.lossyScale.y);
            Vector3 origin = transform.position + Vector3.up * (rayHeight / scale);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, origin + Vector3.down * (rayLength / scale));
        }
    }
}
