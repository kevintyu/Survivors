using Survivors.Enemies;
using Survivors.Stats;
using Survivors.Weapons.Definitions;
using UnityEngine;

namespace Survivors.Weapons
{
    public sealed class ProjectileWeapon : WeaponBehaviour
    {
        private ProjectilePool projectilePool;
        private CharacterStats stats;
        private float cooldownRemaining;

        private ProjectileWeaponDefinition projectileDefinition;

        public override void Initialize(Transform owner, WeaponDefinition definition)
        {
            base.Initialize(owner, definition);

            projectileDefinition = (ProjectileWeaponDefinition)definition;
            stats = owner.GetComponent<CharacterStats>();
            projectilePool = new ProjectilePool(owner, projectileDefinition.ProjectileSprite,
                projectileDefinition.ProjectileSize, projectileDefinition.InitialPoolSize);
            cooldownRemaining = 0f;
        }

        public override void Tick(float deltaTime)
        {
            cooldownRemaining -= deltaTime;
            if (cooldownRemaining > 0f)
            {
                return;
            }

            var target = EnemyTarget.FindClosest(Owner.position);
            if (target == null)
            {
                return;
            }

            Vector2 targetDirection = (target.transform.position - Owner.position).normalized;
            float damage = projectileDefinition.BaseDamage * stats.Get(StatType.DamageMultiplier);
            float projectileSpeed = projectileDefinition.ProjectileSpeed *
                stats.Get(StatType.ProjectileSpeedMultiplier);
            int projectileCount = Mathf.Max(1, projectileDefinition.ProjectileCount +
                Mathf.RoundToInt(stats.Get(StatType.ProjectileCountBonus)));
            const float spreadDegrees = 10f;

            for (int i = 0; i < projectileCount; i++)
            {
                float offset = (i - (projectileCount - 1) * 0.5f) * spreadDegrees;
                Vector2 direction = Quaternion.Euler(0f, 0f, offset) * targetDirection;
                var projectile = projectilePool.Get();
                projectile.Launch(Owner.position, direction, damage, projectileSpeed,
                    projectileDefinition.ProjectileLifetime, projectileDefinition.PiercingCount,
                    projectilePool.Return);
            }

            float cooldownReduction = stats.Get(StatType.CooldownReduction);
            cooldownRemaining = Mathf.Max(0.05f,
                projectileDefinition.AttackInterval * (1f - cooldownReduction));
        }
    }
}
