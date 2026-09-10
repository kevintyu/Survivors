using System;
using Survivors.Combat;
using UnityEngine;

namespace Survivors.Weapons
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class Projectile : MonoBehaviour
    {
        private Vector2 direction;
        private float damage;
        private float speed;
        private float remainingLifetime;
        private Action<Projectile> returnToPool;
        private bool isActive;
        private int remainingHits;

        public void Launch(Vector2 position, Vector2 travelDirection, float projectileDamage,
            float projectileSpeed, float lifetime, int piercingCount,
            Action<Projectile> returnAction)
        {
            transform.SetParent(null, true);
            transform.position = position;
            direction = travelDirection.normalized;
            damage = projectileDamage;
            speed = projectileSpeed;
            remainingLifetime = lifetime;
            remainingHits = piercingCount + 1;
            returnToPool = returnAction;
            isActive = true;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            transform.position += (Vector3)(direction * speed * Time.deltaTime);
            remainingLifetime -= Time.deltaTime;

            if (remainingLifetime <= 0f)
            {
                Release();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive || !other.TryGetComponent<Hurtbox>(out var hurtbox) ||
                hurtbox.Faction != CombatFaction.Enemy)
            {
                return;
            }

            hurtbox.TakeDamage(damage);
            remainingHits--;
            if (remainingHits <= 0)
            {
                Release();
            }
        }

        private void Release()
        {
            if (!isActive)
            {
                return;
            }

            isActive = false;
            returnToPool?.Invoke(this);
        }
    }
}
