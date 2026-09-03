using UnityEngine;

namespace Survivors.Weapons
{
    public abstract class WeaponBehaviour : MonoBehaviour
    {
        protected Transform Owner { get; private set; }

        public virtual void Initialize(Transform owner)
        {
            Owner = owner;
        }

        public abstract void Tick(float deltaTime);
    }
}
