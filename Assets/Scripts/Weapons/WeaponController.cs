using System.Collections.Generic;
using Survivors.Weapons.Definitions;
using UnityEngine;

namespace Survivors.Weapons
{
    public sealed class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponDefinition[] startingWeapons = System.Array.Empty<WeaponDefinition>();
        private readonly List<WeaponBehaviour> weapons = new();

        private void Awake()
        {
            foreach (var definition in startingWeapons)
            {
                Equip(definition);
            }
        }

        public bool Equip(WeaponDefinition definition)
        {
            if (definition == null)
            {
                Debug.LogWarning("A null weapon definition cannot be equipped.", this);
                return false;
            }

            if (!definition.IsValid(out string error))
            {
                Debug.LogWarning($"Weapon was not equipped: {error}", this);
                return false;
            }

            WeaponBehaviour weapon = definition switch
            {
                ProjectileWeaponDefinition => gameObject.AddComponent<ProjectileWeapon>(),
                _ => null
            };

            if (weapon == null)
            {
                Debug.LogWarning($"No behavior supports weapon definition {definition.name}.", this);
                return false;
            }

            weapon.Initialize(transform, definition);
            weapons.Add(weapon);
            return true;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (var weapon in weapons)
            {
                if (weapon.enabled)
                {
                    weapon.Tick(deltaTime);
                }
            }
        }
    }
}
