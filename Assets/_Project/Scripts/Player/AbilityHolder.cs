using System.Collections.Generic;
using UnityEngine;

namespace BiomeWar
{
    // Finds every IAbility on this object and runs them. Adding a new ability
    // means adding a component - no changes here.
    public class AbilityHolder : MonoBehaviour
    {
        readonly List<IAbility> abilities = new List<IAbility>();

        public IReadOnlyList<IAbility> Abilities => abilities;

        void Awake()
        {
            foreach (var a in GetComponents<IAbility>())
            {
                a.Initialise(gameObject);
                abilities.Add(a);
            }
        }

        void Update()
        {
            float dt = Time.deltaTime;
            for (int i = 0; i < abilities.Count; i++)
                abilities[i].Tick(dt);

            if (!InputReader.Exists) return;
            if (GameManager.Exists && GameManager.Instance.CurrentStateId != GameStateId.Playing) return;

            if (InputReader.Instance.Ability1) TryUse(0);
            if (InputReader.Instance.Ability2) TryUse(1);
        }

        void TryUse(int index)
        {
            if (index < 0 || index >= abilities.Count) return;

            var ability = abilities[index];

            if (!ability.IsReady) return;

            ability.Activate();
            GameEvents.RaiseAbilityActivated(ability);
        }
    }
}
