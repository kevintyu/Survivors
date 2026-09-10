using System;
using Survivors.Stats;
using UnityEditor;
using UnityEngine;

namespace Survivors.Editor
{
    public static class StatSystemValidation
    {
        [MenuItem("Survivors/Validate Stat System")]
        public static void Validate()
        {
            var testObject = new GameObject("Stat Validation");

            try
            {
                var stats = testObject.AddComponent<CharacterStats>();
                const string source = "validation-upgrade";

                stats.SetModifier(new StatModifier(
                    StatType.DamageMultiplier, 0.25f, StatModifierOperation.Flat, source));
                stats.SetModifier(new StatModifier(
                    StatType.MoveSpeed, 0.2f, StatModifierOperation.AdditivePercent, source));
                stats.SetModifier(new StatModifier(
                    StatType.CooldownReduction, 0.2f, StatModifierOperation.Flat, source));
                stats.SetModifier(new StatModifier(
                    StatType.MaximumHealth, 50f, StatModifierOperation.Flat, source));

                RequireApproximately(stats.Get(StatType.DamageMultiplier), 1.25f, "damage");
                RequireApproximately(stats.Get(StatType.MoveSpeed), 6f, "movement speed");
                RequireApproximately(stats.Get(StatType.CooldownReduction), 0.2f, "cooldown");
                RequireApproximately(stats.Get(StatType.MaximumHealth), 150f, "maximum health");

                if (!stats.RemoveModifiersFromSource(source))
                {
                    throw new InvalidOperationException("The validation modifiers were not removed.");
                }

                RequireApproximately(stats.Get(StatType.DamageMultiplier), 1f, "reset damage");
                RequireApproximately(stats.Get(StatType.MoveSpeed), 5f, "reset movement speed");
                RequireApproximately(stats.Get(StatType.CooldownReduction), 0f, "reset cooldown");
                RequireApproximately(stats.Get(StatType.MaximumHealth), 100f, "reset health");

                Debug.Log("Character stat calculations and source removal validated successfully.");
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(testObject);
            }
        }

        private static void RequireApproximately(float actual, float expected, string statName)
        {
            if (!Mathf.Approximately(actual, expected))
            {
                throw new InvalidOperationException(
                    $"Unexpected {statName}: expected {expected}, received {actual}.");
            }
        }
    }
}
