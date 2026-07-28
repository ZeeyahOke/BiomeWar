using UnityEngine;

namespace BiomeWar
{
    // For enemies with real animation clips. Parameter names match AC_Mummy.
    public class AnimatorDrivenEnemyAnimator : MonoBehaviour, IEnemyAnimator
    {
        [SerializeField] Animator animator;
        [SerializeField] string movingParam = "IsMoving";
        [SerializeField] string attackParam = "Attack";
        [SerializeField] string hitParam = "Hit";
        [SerializeField] string dieParam = "Die";

        bool lastMoving;

        void Awake()
        {
            if (animator == null) animator = GetComponentInChildren<Animator>();
        }

        public void PlayMove(bool isMoving)
        {
            if (animator == null || isMoving == lastMoving) return;
            lastMoving = isMoving;
            animator.SetBool(movingParam, isMoving);
        }

        public void PlayAttack()
        {
            if (animator != null) animator.SetTrigger(attackParam);
        }

        public void PlayHit()
        {
            if (animator != null) animator.SetTrigger(hitParam);
        }

        public void PlayDeath()
        {
            if (animator == null) return;
            animator.SetBool(movingParam, false);
            animator.SetTrigger(dieParam);
        }
    }
}
