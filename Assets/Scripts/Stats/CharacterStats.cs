using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Stats
{
    public sealed class CharacterStats : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField, Min(0f)] private float baseDamageMultiplier = 1f;
        [SerializeField, Min(0f)] private float baseMoveSpeed = 5f;
        [SerializeField] private float baseCooldownReduction;
        [SerializeField, Min(0.1f)] private float baseProjectileSpeedMultiplier = 1f;
        [SerializeField] private float baseProjectileCountBonus;
        [SerializeField, Min(1f)] private float baseMaximumHealth = 100f;
        [SerializeField, Min(0.1f)] private float basePickupRadius = 1.25f;

        private readonly List<StatModifier> modifiers = new();
        private readonly Dictionary<StatType, float> finalValues = new();

        public event Action Changed;

        private void Awake()
        {
            Recalculate();
        }

        public float Get(StatType type)
        {
            if (!finalValues.TryGetValue(type, out float value))
            {
                Recalculate();
                value = finalValues[type];
            }

            return value;
        }

        public void SetModifier(StatModifier modifier)
        {
            modifiers.RemoveAll(existing =>
                existing.Type == modifier.Type && existing.SourceId == modifier.SourceId);
            modifiers.Add(modifier);
            Recalculate();
        }

        public bool RemoveModifiersFromSource(string sourceId)
        {
            int removed = modifiers.RemoveAll(modifier => modifier.SourceId == sourceId);
            if (removed > 0)
            {
                Recalculate();
            }

            return removed > 0;
        }

        private void Recalculate()
        {
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                finalValues[type] = Calculate(type);
            }

            Changed?.Invoke();
        }

        private float Calculate(StatType type)
        {
            float flat = 0f;
            float additivePercent = 0f;
            float multiplicativeFactor = 1f;

            foreach (var modifier in modifiers)
            {
                if (modifier.Type != type)
                {
                    continue;
                }

                switch (modifier.Operation)
                {
                    case StatModifierOperation.Flat:
                        flat += modifier.Amount;
                        break;
                    case StatModifierOperation.AdditivePercent:
                        additivePercent += modifier.Amount;
                        break;
                    case StatModifierOperation.MultiplicativePercent:
                        multiplicativeFactor *= 1f + modifier.Amount;
                        break;
                }
            }

            float result = (GetBaseValue(type) + flat) * (1f + additivePercent) * multiplicativeFactor;
            return Clamp(type, result);
        }

        private float GetBaseValue(StatType type)
        {
            return type switch
            {
                StatType.DamageMultiplier => baseDamageMultiplier,
                StatType.MoveSpeed => baseMoveSpeed,
                StatType.CooldownReduction => baseCooldownReduction,
                StatType.ProjectileSpeedMultiplier => baseProjectileSpeedMultiplier,
                StatType.ProjectileCountBonus => baseProjectileCountBonus,
                StatType.MaximumHealth => baseMaximumHealth,
                StatType.PickupRadius => basePickupRadius,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private static float Clamp(StatType type, float value)
        {
            return type switch
            {
                StatType.DamageMultiplier => Mathf.Max(0f, value),
                StatType.MoveSpeed => Mathf.Max(0f, value),
                StatType.CooldownReduction => Mathf.Clamp(value, -10f, 0.9f),
                StatType.ProjectileSpeedMultiplier => Mathf.Max(0.1f, value),
                StatType.ProjectileCountBonus => value,
                StatType.MaximumHealth => Mathf.Max(1f, value),
                StatType.PickupRadius => Mathf.Max(0.1f, value),
                _ => value
            };
        }
    }
}
