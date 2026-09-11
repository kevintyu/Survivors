using Survivors.Combat;
using Survivors.UI;
using Survivors.Pickups;
using UnityEngine;

namespace Survivors.Enemies
{
    public sealed class PrototypeEnemySpawner : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float respawnDelay = 2f;
        [SerializeField, Min(1f)] private float spawnDistance = 4f;

        private Transform player;
        private Sprite enemySprite;
        private ExperienceGemPool experienceGemPool;
        private float respawnAt = -1f;

        public void Configure(Transform playerTarget, Sprite sprite, ExperienceGemPool gemPool)
        {
            player = playerTarget;
            enemySprite = sprite;
            experienceGemPool = gemPool;
        }

        public void SpawnImmediately()
        {
            if (player == null || EnemyTarget.Count > 0)
            {
                return;
            }

            CreateEnemy();
            respawnAt = -1f;
        }

        private void Update()
        {
            if (player == null || EnemyTarget.Count > 0)
            {
                respawnAt = -1f;
                return;
            }

            if (respawnAt < 0f)
            {
                respawnAt = Time.time + respawnDelay;
            }

            if (Time.time >= respawnAt)
            {
                CreateEnemy();
                respawnAt = -1f;
            }
        }

        private void CreateEnemy()
        {
            Vector2 spawnDirection = Random.insideUnitCircle.normalized;
            if (spawnDirection == Vector2.zero)
            {
                spawnDirection = Vector2.right;
            }

            var enemy = new GameObject("Enemy");
            enemy.layer = CombatLayers.Enemy;
            enemy.transform.position = (Vector2)player.position + spawnDirection * spawnDistance;

            var renderer = enemy.AddComponent<SpriteRenderer>();
            renderer.sprite = enemySprite;
            renderer.color = new Color(1f, 0.25f, 0.25f);

            var body = enemy.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            enemy.AddComponent<BoxCollider2D>();
            var health = enemy.AddComponent<Health>();
            health.Configure(30f, true);
            enemy.AddComponent<EnemyExperienceDrop>().Configure(health, experienceGemPool, 5);
            Hurtbox.Create(enemy, health, CombatFaction.Enemy, CombatLayers.Enemy, Vector2.one);
            ContactHitbox.Create(enemy, 10f, 0.75f, CombatFaction.Player,
                CombatLayers.EnemyContact, Vector2.one * 1.05f);
            enemy.AddComponent<WorldHealthBar>().Configure(
                new Color(1f, 0.3f, 0.2f), new Vector2(0f, 0.7f));
            enemy.AddComponent<EnemyTarget>();
            enemy.AddComponent<EnemyMovement>().SetTarget(player);
        }
    }
}
