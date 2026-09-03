using Survivors.Weapons.Definitions;
using UnityEngine;

namespace Survivors.Weapons
{
    public abstract class WeaponBehaviour : MonoBehaviour
    {
        protected Transform Owner { get; private set; }
        public WeaponDefinition Definition { get; private set; }

        public virtual void Initialize(Transform owner, WeaponDefinition definition)
        {
            Owner = owner;
            Definition = definition;
        }

        public abstract void Tick(float deltaTime);
    }
}
