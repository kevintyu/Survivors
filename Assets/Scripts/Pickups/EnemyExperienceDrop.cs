using Survivors.Combat;
using UnityEngine;

namespace Survivors.Pickups
{
    public sealed class EnemyExperienceDrop : MonoBehaviour
    {
        private Health health;
        private ExperienceGemPool pool;
        private int experienceValue;

        public void Configure(Health enemyHealth, ExperienceGemPool gemPool, int value)
        {
            health = enemyHealth;
            pool = gemPool;
            experienceValue = Mathf.Max(1, value);
            health.Died += DropExperience;
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died -= DropExperience;
            }
        }

        private void DropExperience()
        {
            pool.Spawn(transform.position, experienceValue);
        }
    }
}
