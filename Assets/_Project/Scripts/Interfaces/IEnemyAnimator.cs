namespace BiomeWar
{
    public interface IEnemyAnimator
    {
        void PlayMove(bool isMoving);
        void PlayAttack();
        void PlayHit();
        void PlayDeath();
    }
}
