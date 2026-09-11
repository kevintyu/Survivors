using Survivors.Progression;
using UnityEngine;

namespace Survivors.Pickups
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class ExperienceGem : MonoBehaviour
    {
        private ExperienceGemPool pool;
        private int value;
        private bool isAvailable;

        public void Spawn(Vector2 position, int experienceValue, ExperienceGemPool ownerPool)
        {
            transform.SetParent(null, true);
            transform.position = position;
            value = Mathf.Max(1, experienceValue);
            pool = ownerPool;
            isAvailable = true;
            gameObject.SetActive(true);
        }

        public bool TryCollect(PlayerExperience experience)
        {
            if (!isAvailable || experience == null)
            {
                return false;
            }

            isAvailable = false;
            experience.AddExperience(value);
            pool.Return(this);
            return true;
        }
    }
}
