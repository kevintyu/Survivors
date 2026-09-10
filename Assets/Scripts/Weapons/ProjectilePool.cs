using System.Collections.Generic;
using Survivors.Combat;
using UnityEngine;

namespace Survivors.Weapons
{
    public sealed class ProjectilePool
    {
        private readonly Stack<Projectile> available = new();
        private readonly Transform container;
        private readonly Sprite sprite;
        private readonly float projectileSize;

        public ProjectilePool(Transform owner, Sprite projectileSprite, float size, int initialSize)
        {
            container = new GameObject("Projectile Pool").transform;
            container.SetParent(owner);
            sprite = projectileSprite;
            projectileSize = size;

            for (int i = 0; i < initialSize; i++)
            {
                available.Push(CreateProjectile());
            }
        }

        public Projectile Get()
        {
            return available.Count > 0 ? available.Pop() : CreateProjectile();
        }

        public void Return(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            projectile.transform.SetParent(container);
            available.Push(projectile);
        }

        private Projectile CreateProjectile()
        {
            var projectileObject = new GameObject("Projectile");
            projectileObject.layer = CombatLayers.PlayerProjectile;
            projectileObject.transform.SetParent(container);
            projectileObject.transform.localScale = Vector3.one * projectileSize;

            var renderer = projectileObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = new Color(1f, 0.85f, 0.1f);
            renderer.sortingOrder = 1;

            var body = projectileObject.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;

            var collider = projectileObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;

            var projectile = projectileObject.AddComponent<Projectile>();
            projectileObject.SetActive(false);
            return projectile;
        }
    }
}
