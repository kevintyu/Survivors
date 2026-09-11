using Survivors.Progression;
using Survivors.Stats;
using UnityEngine;

namespace Survivors.Pickups
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class ExperienceCollector : MonoBehaviour
    {
        private PlayerExperience experience;
        private CharacterStats stats;
        private CircleCollider2D collectionTrigger;

        public static ExperienceCollector Create(GameObject owner, PlayerExperience playerExperience,
            CharacterStats characterStats, int layer)
        {
            var collectorObject = new GameObject("Experience Collector");
            collectorObject.layer = layer;
            collectorObject.transform.SetParent(owner.transform, false);

            var collider = collectorObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;

            var collector = collectorObject.AddComponent<ExperienceCollector>();
            collector.experience = playerExperience;
            collector.stats = characterStats;
            collector.collectionTrigger = collider;
            collector.stats.Changed += collector.ApplyRadius;
            collector.ApplyRadius();
            return collector;
        }

        private void OnDestroy()
        {
            if (stats != null)
            {
                stats.Changed -= ApplyRadius;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<ExperienceGem>(out var gem))
            {
                gem.TryCollect(experience);
            }
        }

        private void ApplyRadius()
        {
            collectionTrigger.radius = stats.Get(StatType.PickupRadius);
        }
    }
}
