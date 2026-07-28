using UnityEngine;

namespace BiomeWar
{
    public class DashAbility : AbilityBase
    {
        [SerializeField] float dashDistance = 6f;
        [SerializeField] float dashDuration = 0.15f;

        PlayerController controller;
        float timeLeft;
        Vector3 direction;

        public override void Initialise(GameObject ownerObject)
        {
            base.Initialise(ownerObject);
            controller = ownerObject.GetComponent<PlayerController>();
            displayName = "Dash";
        }

        protected override void Execute()
        {
            Vector2 input = InputReader.Exists ? InputReader.Instance.Move : Vector2.zero;

            direction = input.sqrMagnitude > 0.01f
                ? (owner.transform.right * input.x + owner.transform.forward * input.y).normalized
                : owner.transform.forward;

            timeLeft = dashDuration;
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (timeLeft <= 0f || controller == null) return;

            float step = (dashDistance / dashDuration) * deltaTime;
            controller.ApplyImpulse(direction * step);
            timeLeft -= deltaTime;
        }
    }
}
