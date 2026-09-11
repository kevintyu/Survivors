using System.Collections.Generic;
using Survivors.Combat;
using UnityEngine;

namespace Survivors.Pickups
{
    public sealed class ExperienceGemPool : MonoBehaviour
    {
        [SerializeField, Min(0)] private int initialSize = 8;

        private readonly Stack<ExperienceGem> available = new();
        private Transform container;
        private Sprite gemSprite;
        private bool initialized;

        public void Configure(Sprite sprite)
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            gemSprite = sprite;
            container = new GameObject("Experience Gem Pool").transform;
            container.SetParent(transform);

            for (int i = 0; i < initialSize; i++)
            {
                available.Push(CreateGem());
            }
        }

        public void Spawn(Vector2 position, int value)
        {
            if (!initialized)
            {
                Debug.LogError("Experience gem pool has not been configured.", this);
                return;
            }

            ExperienceGem gem = available.Count > 0 ? available.Pop() : CreateGem();
            gem.Spawn(position, value, this);
        }

        public void Return(ExperienceGem gem)
        {
            gem.gameObject.SetActive(false);
            gem.transform.SetParent(container, false);
            available.Push(gem);
        }

        private ExperienceGem CreateGem()
        {
            var gemObject = new GameObject("Experience Gem");
            gemObject.layer = CombatLayers.Player;
            gemObject.transform.SetParent(container, false);
            gemObject.transform.localScale = Vector3.one * 0.22f;

            var renderer = gemObject.AddComponent<SpriteRenderer>();
            renderer.sprite = gemSprite;
            renderer.color = new Color(0.1f, 0.95f, 1f);
            renderer.sortingOrder = 2;

            var collider = gemObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;

            var gem = gemObject.AddComponent<ExperienceGem>();
            gemObject.SetActive(false);
            return gem;
        }
    }
}
