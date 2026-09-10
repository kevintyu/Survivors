using Survivors.Combat;
using Survivors.Stats;
using UnityEngine;

namespace Survivors.Player
{
    [RequireComponent(typeof(Health), typeof(CharacterStats))]
    public sealed class PlayerHealthStats : MonoBehaviour
    {
        private Health health;
        private CharacterStats stats;

        private void Awake()
        {
            health = GetComponent<Health>();
            stats = GetComponent<CharacterStats>();
            stats.Changed += ApplyMaximumHealth;
            ApplyMaximumHealth();
        }

        private void OnDestroy()
        {
            if (stats != null)
            {
                stats.Changed -= ApplyMaximumHealth;
            }
        }

        private void ApplyMaximumHealth()
        {
            health.SetMaximumHealth(stats.Get(StatType.MaximumHealth), true);
        }
    }
}
