using UnityEngine;

namespace BiomeWar
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float gravity = -20f;

        [Header("Look")]
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float mouseSensitivity = 0.12f;
        [SerializeField] private float touchSensitivity = 0.25f;
        [SerializeField] private float pitchClamp = 85f;

        private CharacterController cc;
        private float pitch;
        private Vector3 velocity;
        private bool canControl = true;

        public Transform CameraPivot => cameraPivot;

        void Awake()
        {
            cc = GetComponent<CharacterController>();
            if (cameraPivot == null && Camera.main != null)
                cameraPivot = Camera.main.transform;
        }

        void OnEnable()
        {
            GameEvents.OnGameStateChanged += OnStateChanged;
            GameEvents.OnPlayerDied += OnDied;
        }

        void OnDisable()
        {
            GameEvents.OnGameStateChanged -= OnStateChanged;
            GameEvents.OnPlayerDied -= OnDied;
        }

        void OnStateChanged(GameStateId id) => canControl = (id == GameStateId.Playing);
        void OnDied() => canControl = false;

        void Update()
        {
            if (!canControl || !InputReader.Exists) return;
            Look();
            Move();
        }

        void Look()
        {
            Vector2 look = InputReader.Instance.Look;

#if UNITY_ANDROID || UNITY_IOS
            float sens = touchSensitivity;
#else
            float sens = mouseSensitivity;
#endif

            transform.Rotate(Vector3.up * look.x * sens);

            pitch = Mathf.Clamp(pitch - look.y * sens, -pitchClamp, pitchClamp);
            if (cameraPivot != null)
                cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        void Move()
        {
            Vector2 input = InputReader.Instance.Move;
            Vector3 dir = transform.right * input.x + transform.forward * input.y;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            if (cc.isGrounded && velocity.y < 0f)
                velocity.y = -2f;

            velocity.y += gravity * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
        }

        // Used by DashAbility
        public void ApplyImpulse(Vector3 delta) => cc.Move(delta);
    }
}
