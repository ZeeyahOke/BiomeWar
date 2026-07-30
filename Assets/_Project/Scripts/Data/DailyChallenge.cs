using System;
using System.Collections.Generic;

namespace BiomeWar
{
    [Serializable]
    public class ChallengeModifier
    {
        public string id = "standard";
        public string name = "Standard Deployment";
        public string description = "No modifiers active.";
        public float enemySpeedMultiplier = 1f;
        public float enemyHealthMultiplier = 1f;
        public float enemyDamageMultiplier = 1f;
        public float scoreMultiplier = 1f;

        public static ChallengeModifier Default() => new ChallengeModifier();
    }

    [Serializable]
    public class ChallengeList
    {
        public List<ChallengeModifier> modifiers = new List<ChallengeModifier>();
    }
}
