using UnityEngine;

namespace Survivors.Weapons
{
    public sealed class WeaponController : MonoBehaviour
    {
        private WeaponBehaviour[] weapons;

        private void Awake()
        {
            weapons = GetComponents<WeaponBehaviour>();
            foreach (var weapon in weapons)
            {
                weapon.Initialize(transform);
            }
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
