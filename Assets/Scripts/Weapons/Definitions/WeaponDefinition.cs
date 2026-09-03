using UnityEngine;

namespace Survivors.Weapons.Definitions
{
    public abstract class WeaponDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite icon;

        [Header("Combat")]
        [SerializeField, Min(0f)] private float baseDamage = 10f;
        [SerializeField, Min(0.05f)] private float attackInterval = 1f;
        [SerializeField, Min(1)] private int projectileCount = 1;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public float BaseDamage => baseDamage;
        public float AttackInterval => attackInterval;
        public int ProjectileCount => projectileCount;

        public virtual bool IsValid(out string error)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                error = $"{name} has no stable weapon ID.";
                return false;
            }

            if (attackInterval <= 0f || projectileCount < 1)
            {
                error = $"{name} has invalid attack timing or projectile count.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        public void ConfigureBase(string weaponId, string weaponName, string weaponDescription,
            Sprite weaponIcon, float damage, float interval, int count)
        {
            id = weaponId;
            displayName = weaponName;
            description = weaponDescription;
            icon = weaponIcon;
            baseDamage = Mathf.Max(0f, damage);
            attackInterval = Mathf.Max(0.05f, interval);
            projectileCount = Mathf.Max(1, count);
        }
    }
}
