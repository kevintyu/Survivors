using UnityEngine;

namespace Survivors.Weapons.Definitions
{
    [CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "Survivors/Weapons/Projectile Weapon")]
    public sealed class ProjectileWeaponDefinition : WeaponDefinition
    {
        [Header("Projectile")]
        [SerializeField] private Sprite projectileSprite;
        [SerializeField, Min(0.1f)] private float projectileSpeed = 8f;
        [SerializeField, Min(0.1f)] private float projectileLifetime = 3f;
        [SerializeField, Min(0.05f)] private float projectileSize = 0.25f;
        [SerializeField, Min(0)] private int piercingCount;
        [SerializeField, Min(0)] private int initialPoolSize = 8;

        public Sprite ProjectileSprite => projectileSprite;
        public float ProjectileSpeed => projectileSpeed;
        public float ProjectileLifetime => projectileLifetime;
        public float ProjectileSize => projectileSize;
        public int PiercingCount => piercingCount;
        public int InitialPoolSize => initialPoolSize;

        public override bool IsValid(out string error)
        {
            if (!base.IsValid(out error))
            {
                return false;
            }

            if (projectileSprite == null)
            {
                error = $"{name} has no projectile sprite.";
                return false;
            }

            if (projectileSpeed <= 0f || projectileLifetime <= 0f || projectileSize <= 0f)
            {
                error = $"{name} has invalid projectile movement or size values.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        public void ConfigureProjectile(Sprite sprite, float speed, float lifetime,
            float size, int pierce, int poolSize)
        {
            projectileSprite = sprite;
            projectileSpeed = Mathf.Max(0.1f, speed);
            projectileLifetime = Mathf.Max(0.1f, lifetime);
            projectileSize = Mathf.Max(0.05f, size);
            piercingCount = Mathf.Max(0, pierce);
            initialPoolSize = Mathf.Max(0, poolSize);
        }
    }
}
