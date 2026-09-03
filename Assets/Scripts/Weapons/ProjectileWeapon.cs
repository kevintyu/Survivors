using Survivors.Enemies;
using Survivors.Weapons.Definitions;
using UnityEngine;

namespace Survivors.Weapons
{
    public sealed class ProjectileWeapon : WeaponBehaviour
    {
        private ProjectilePool projectilePool;
        private float cooldownRemaining;

        // Runtime values are copied from the asset so gameplay never mutates shared content data.
        private float damage;
        private float attackInterval;
        private int projectileCount;
        private float projectileSpeed;
        private float projectileLifetime;
        private int piercingCount;

        public override void Initialize(Transform owner, WeaponDefinition definition)
        {
            base.Initialize(owner, definition);

            var projectileDefinition = (ProjectileWeaponDefinition)definition;
            damage = projectileDefinition.BaseDamage;
            attackInterval = projectileDefinition.AttackInterval;
            projectileCount = projectileDefinition.ProjectileCount;
            projectileSpeed = projectileDefinition.ProjectileSpeed;
            projectileLifetime = projectileDefinition.ProjectileLifetime;
            piercingCount = projectileDefinition.PiercingCount;
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
            const float spreadDegrees = 10f;

            for (int i = 0; i < projectileCount; i++)
            {
                float offset = (i - (projectileCount - 1) * 0.5f) * spreadDegrees;
                Vector2 direction = Quaternion.Euler(0f, 0f, offset) * targetDirection;
                var projectile = projectilePool.Get();
                projectile.Launch(Owner.position, direction, damage, projectileSpeed,
                    projectileLifetime, piercingCount, projectilePool.Return);
            }

            cooldownRemaining = attackInterval;
        }
    }
}
