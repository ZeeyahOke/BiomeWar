using UnityEngine;
using UnityEngine.InputSystem;

namespace BiomeWar
{
    /// <summary>Central input source. Mobile on-screen controls write into the virtual fields.</summary>
    public class InputReader : ManagerBase<InputReader>
    {
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Fire { get; private set; }
        public bool Interact { get; private set; }
        public bool Ability1 { get; private set; }
        public bool Ability2 { get; private set; }
        public bool PausePressed { get; private set; }

        // Mobile virtual input
        public Vector2 VirtualMove;
        public Vector2 VirtualLook;
        public bool VirtualFire, VirtualInteract, VirtualAbility1, VirtualAbility2;

        private void Update()
        {
#if UNITY_ANDROID || UNITY_IOS
            Move = VirtualMove;
            Look = VirtualLook;
            Fire = VirtualFire;
            Interact = VirtualInteract;
            Ability1 = VirtualAbility1;
            Ability2 = VirtualAbility2;
            PausePressed = false;

            VirtualInteract = VirtualAbility1 = VirtualAbility2 = false;
            VirtualLook = Vector2.zero;
    #else
        var kb = Keyboard.current;
        var mouse = Mouse.current;

        Vector2 move = Vector2.zero;
        if (kb != null)
        {
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) move.y += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) move.y -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) move.x += 1f;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) move.x -= 1f;

            Interact = kb.eKey.wasPressedThisFrame;
            Ability1 = kb.qKey.wasPressedThisFrame;
            Ability2 = kb.fKey.wasPressedThisFrame;
            PausePressed = kb.escapeKey.wasPressedThisFrame;
        }

        Move = move.normalized;
        Look = mouse != null ? mouse.delta.ReadValue() : Vector2.zero;
        Fire = mouse != null && mouse.leftButton.isPressed;
    #endif
        }
    }
}
