using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class ContactHitbox : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float damage = 10f;
        [SerializeField, Min(0.05f)] private float damageInterval = 0.75f;
        [SerializeField] private CombatFaction targetFaction = CombatFaction.Player;

        private readonly Dictionary<Hurtbox, float> nextDamageTimes = new();

        public static ContactHitbox Create(GameObject owner, float amount, float interval,
            CombatFaction faction, int layer, Vector2 size)
        {
            var hitboxObject = new GameObject("Contact Hitbox");
            hitboxObject.layer = layer;
            hitboxObject.transform.SetParent(owner.transform, false);

            var collider = hitboxObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = size;

            var hitbox = hitboxObject.AddComponent<ContactHitbox>();
            hitbox.damage = Mathf.Max(0f, amount);
            hitbox.damageInterval = Mathf.Max(0.05f, interval);
            hitbox.targetFaction = faction;
            return hitbox;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent<Hurtbox>(out var hurtbox) ||
                hurtbox.Faction != targetFaction)
            {
                return;
            }

            if (nextDamageTimes.TryGetValue(hurtbox, out float nextTime) && Time.time < nextTime)
            {
                return;
            }

            hurtbox.TakeDamage(damage);
            nextDamageTimes[hurtbox] = Time.time + damageInterval;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Hurtbox>(out var hurtbox))
            {
                nextDamageTimes.Remove(hurtbox);
            }
        }

        private void OnDisable()
        {
            nextDamageTimes.Clear();
        }
    }
}
