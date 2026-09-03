using Survivors.Enemies;
using UnityEngine;

namespace Survivors.Weapons
{
    public sealed class ProjectileWeapon : WeaponBehaviour
    {
        [SerializeField, Min(0.05f)] private float attackInterval = 1f;
        [SerializeField, Min(0f)] private float damage = 10f;
        [SerializeField, Min(0.1f)] private float projectileSpeed = 8f;
        [SerializeField, Min(0.1f)] private float projectileLifetime = 3f;
        [SerializeField, Min(0)] private int initialPoolSize = 8;

        private Sprite projectileSprite;
        private ProjectilePool projectilePool;
        private float cooldownRemaining;

        public void Configure(Sprite sprite)
        {
            projectileSprite = sprite;
        }

        public override void Initialize(Transform owner)
        {
            base.Initialize(owner);
            projectilePool = new ProjectilePool(owner, projectileSprite, initialPoolSize);
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

            Vector2 direction = (target.transform.position - Owner.position).normalized;
            var projectile = projectilePool.Get();
            projectile.Launch(Owner.position, direction, damage, projectileSpeed,
                projectileLifetime, projectilePool.Return);
            cooldownRemaining = attackInterval;
        }
    }
}
