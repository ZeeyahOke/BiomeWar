using UnityEngine;

namespace BiomeWar
{
    // Bridges the player's death to the game state machine.
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField] float delayBeforeGameOver = 1.5f;

        void OnEnable() => GameEvents.OnPlayerDied += OnDied;
        void OnDisable() => GameEvents.OnPlayerDied -= OnDied;

        void OnDied()
        {
            if (SaveManager.Exists) SaveManager.Instance.RecordDeath();
            Invoke(nameof(ShowGameOver), delayBeforeGameOver);
        }

        void ShowGameOver()
        {
            if (GameManager.Exists)
                GameManager.Instance.ChangeState(GameManager.Instance.GameOver);
        }
    }
}
