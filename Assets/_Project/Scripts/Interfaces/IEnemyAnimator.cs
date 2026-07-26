namespace BiomeWar
{
    /// <summary>
    /// Abstracts how an enemy is visually animated.
    /// Some assets ship with animation clips, others (the Snowman) have no rig
    /// at all and are animated procedurally. Behaviour code calls this interface
    /// and is unaffected by which driver is used.
    /// </summary>
    public interface IEnemyAnimator
    {
        void PlayMove(bool isMoving);
        void PlayAttack();
        void PlayHit();
        void PlayDeath();
    }
}
