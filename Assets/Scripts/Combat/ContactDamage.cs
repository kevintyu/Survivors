using UnityEngine;

namespace Survivors.Combat
{
    public sealed class ContactDamage : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float damage = 10f;
        [SerializeField, Min(0.05f)] private float damageInterval = 0.75f;

        private float nextDamageTime;

        public void Configure(float newDamage, float newDamageInterval)
        {
            damage = Mathf.Max(0f, newDamage);
            damageInterval = Mathf.Max(0.05f, newDamageInterval);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (Time.time < nextDamageTime ||
                !collision.gameObject.TryGetComponent<IDamageable>(out var target))
            {
                return;
            }

            target.TakeDamage(damage);
            nextDamageTime = Time.time + damageInterval;
        }
    }
}
