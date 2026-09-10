using System;
using UnityEngine;

namespace Survivors.Combat
{
    public sealed class Health : MonoBehaviour, IDamageable
    {
        [SerializeField, Min(1f)] private float maximumHealth = 100f;
        [SerializeField] private bool destroyOnDeath;

        public float Current { get; private set; }
        public float Maximum => maximumHealth;
        public bool IsDead { get; private set; }

        public event Action<float, float> Changed;
        public event Action Died;

        private void Awake()
        {
            Current = maximumHealth;
        }

        public void Configure(float newMaximumHealth, bool shouldDestroyOnDeath)
        {
            maximumHealth = Mathf.Max(1f, newMaximumHealth);
            destroyOnDeath = shouldDestroyOnDeath;
            Current = maximumHealth;
            IsDead = false;
            Changed?.Invoke(Current, maximumHealth);
        }

        public void SetMaximumHealth(float newMaximumHealth, bool healByIncrease)
        {
            float previousMaximum = maximumHealth;
            maximumHealth = Mathf.Max(1f, newMaximumHealth);

            if (!IsDead && healByIncrease && maximumHealth > previousMaximum)
            {
                Current += maximumHealth - previousMaximum;
            }

            Current = Mathf.Clamp(Current, 0f, maximumHealth);
            Changed?.Invoke(Current, maximumHealth);
        }

        public void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0f)
            {
                return;
            }

            Current = Mathf.Max(0f, Current - amount);
            Changed?.Invoke(Current, maximumHealth);

            if (Current > 0f)
            {
                return;
            }

            IsDead = true;
            Died?.Invoke();

            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }
    }
}
