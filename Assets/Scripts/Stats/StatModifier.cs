using System;

namespace Survivors.Stats
{
    public readonly struct StatModifier
    {
        public StatType Type { get; }
        public float Amount { get; }
        public StatModifierOperation Operation { get; }
        public string SourceId { get; }

        public StatModifier(StatType type, float amount, StatModifierOperation operation,
            string sourceId)
        {
            if (string.IsNullOrWhiteSpace(sourceId))
            {
                throw new ArgumentException("A stat modifier requires a source ID.", nameof(sourceId));
            }

            Type = type;
            Amount = amount;
            Operation = operation;
            SourceId = sourceId;
        }
    }
}
