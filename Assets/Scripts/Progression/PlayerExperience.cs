using System;
using UnityEngine;

namespace Survivors.Progression
{
    public sealed class PlayerExperience : MonoBehaviour
    {
        public int Level { get; private set; } = 1;
        public int CurrentExperience { get; private set; }
        public int ExperienceForNextLevel => CalculateRequirement(Level);

        public event Action<int, int, int> ProgressChanged;
        public event Action<int> LevelGained;

        public void AddExperience(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            CurrentExperience += amount;

            while (CurrentExperience >= ExperienceForNextLevel)
            {
                CurrentExperience -= ExperienceForNextLevel;
                Level++;
                LevelGained?.Invoke(Level);
                Debug.Log($"Player reached level {Level}.", this);
            }

            ProgressChanged?.Invoke(Level, CurrentExperience, ExperienceForNextLevel);
        }

        public static int CalculateRequirement(int level)
        {
            return Mathf.Max(1, Mathf.CeilToInt(10f * Mathf.Pow(Mathf.Max(1, level), 1.5f)));
        }
    }
}
