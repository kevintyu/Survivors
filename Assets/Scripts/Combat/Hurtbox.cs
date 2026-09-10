using UnityEngine;

namespace Survivors.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class Hurtbox : MonoBehaviour
    {
        private IDamageable damageable;

        public CombatFaction Faction { get; private set; }

        public static Hurtbox Create(GameObject owner, Health health, CombatFaction faction,
            int layer, Vector2 size)
        {
            var hurtboxObject = new GameObject("Hurtbox");
            hurtboxObject.layer = layer;
            hurtboxObject.transform.SetParent(owner.transform, false);

            var collider = hurtboxObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = size;

            var hurtbox = hurtboxObject.AddComponent<Hurtbox>();
            hurtbox.Faction = faction;
            hurtbox.damageable = health;
            return hurtbox;
        }

        public void TakeDamage(float amount)
        {
            damageable?.TakeDamage(amount);
        }
    }
}
