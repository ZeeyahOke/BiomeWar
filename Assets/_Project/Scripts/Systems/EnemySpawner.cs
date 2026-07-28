using System.Collections.Generic;
using UnityEngine;

namespace BiomeWar
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] LevelConfig config;
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] Transform bossSpawnPoint;
        [SerializeField] float spawnRadius = 2f;

        readonly List<GameObject> spawned = new List<GameObject>();

        void Start()
        {
            if (config == null || spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogError("EnemySpawner needs a LevelConfig and at least one spawn point.");
                return;
            }

            SpawnGroups();
            SpawnBoss();
        }

        void SpawnGroups()
        {
            int pointIndex = 0;

            foreach (var group in config.SpawnGroups)
            {
                if (group.Config == null || group.Config.Prefab == null) continue;

                for (int i = 0; i < group.Count; i++)
                {
                    Transform point = spawnPoints[pointIndex % spawnPoints.Length];
                    pointIndex++;

                    Vector2 offset = Random.insideUnitCircle * spawnRadius;
                    Vector3 pos = point.position + new Vector3(offset.x, 0f, offset.y);

                    var go = Instantiate(group.Config.Prefab, pos, point.rotation);
                    spawned.Add(go);
                }
            }
        }

        void SpawnBoss()
        {
            if (!config.HasBoss || config.BossConfig == null || config.BossConfig.Prefab == null) return;

            Transform point = bossSpawnPoint != null ? bossSpawnPoint : spawnPoints[0];
            var go = Instantiate(config.BossConfig.Prefab, point.position, point.rotation);

            var controller = go.GetComponent<EnemyController>();
            if (controller != null) controller.MarkAsBoss();

            spawned.Add(go);
        }

        void OnDrawGizmos()
        {
            if (spawnPoints == null) return;

            Gizmos.color = Color.magenta;
            foreach (var p in spawnPoints)
                if (p != null) Gizmos.DrawWireSphere(p.position, spawnRadius);

            if (bossSpawnPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(bossSpawnPoint.position, spawnRadius * 1.5f);
            }
        }
    }
}
