using System;
using Survivors.Combat;
using Survivors.Pickups;
using Survivors.Progression;
using Survivors.Stats;
using UnityEditor;
using UnityEngine;

namespace Survivors.Editor
{
    public static class ExperienceSystemValidation
    {
        [MenuItem("Survivors/Validate Experience System")]
        public static void Validate()
        {
            var playerObject = new GameObject("Experience Validation Player");

            try
            {
                var stats = playerObject.AddComponent<CharacterStats>();
                var experience = playerObject.AddComponent<PlayerExperience>();
                var collector = ExperienceCollector.Create(
                    playerObject, experience, stats, CombatLayers.Player);
                var collectionTrigger = collector.GetComponent<CircleCollider2D>();

                RequireApproximately(collectionTrigger.radius, 1.25f, "base pickup radius");
                stats.SetModifier(new StatModifier(StatType.PickupRadius, 0.75f,
                    StatModifierOperation.Flat, "validation-pickup-radius"));
                RequireApproximately(collectionTrigger.radius, 2f, "modified pickup radius");

                int levelUpEvents = 0;
                experience.LevelGained += _ => levelUpEvents++;
                experience.AddExperience(10);
                Require(experience.Level == 2, "10 XP did not advance the player to level 2.");
                Require(experience.CurrentExperience == 0, "Excess XP at level 2 is incorrect.");
                Require(experience.ExperienceForNextLevel == 29,
                    "The level 2 requirement does not match the configured curve.");

                experience.AddExperience(29);
                Require(experience.Level == 3, "29 XP did not advance the player to level 3.");
                Require(levelUpEvents == 2, "Level-up events were not raised exactly once per level.");

                Debug.Log("Experience thresholds, events, and pickup-radius updates validated successfully.");
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(playerObject);
            }
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        private static void RequireApproximately(float actual, float expected, string label)
        {
            if (!Mathf.Approximately(actual, expected))
            {
                throw new InvalidOperationException(
                    $"Unexpected {label}: expected {expected}, received {actual}.");
            }
        }
    }
}
