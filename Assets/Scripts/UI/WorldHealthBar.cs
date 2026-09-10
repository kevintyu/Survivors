using Survivors.Combat;
using UnityEngine;

namespace Survivors.UI
{
    [RequireComponent(typeof(Health))]
    public sealed class WorldHealthBar : MonoBehaviour
    {
        [SerializeField] private Vector2 offset = new(0f, 0.7f);
        [SerializeField, Min(0.1f)] private float width = 1f;
        [SerializeField, Min(0.02f)] private float height = 0.12f;
        [SerializeField] private Color fillColor = new(0.2f, 0.9f, 0.3f);

        private static Sprite barSprite;

        private Health health;
        private Transform fillTransform;
        private SpriteRenderer fillRenderer;

        private void Awake()
        {
            health = GetComponent<Health>();
            CreateVisuals();
            health.Changed += UpdateBar;
            UpdateBar(health.Current, health.Maximum);
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Changed -= UpdateBar;
            }
        }

        public void Configure(Color color, Vector2 localOffset)
        {
            fillColor = color;
            offset = localOffset;

            if (fillRenderer != null)
            {
                fillRenderer.color = fillColor;
                fillTransform.parent.localPosition = offset;
            }
        }

        private void CreateVisuals()
        {
            var root = new GameObject("Health Bar").transform;
            root.SetParent(transform, false);
            root.localPosition = offset;

            var background = new GameObject("Background");
            background.transform.SetParent(root, false);
            background.transform.localPosition = new Vector3(-width * 0.55f, 0f, 0f);
            background.transform.localScale = new Vector3(width * 1.1f, height * 1.5f, 1f);

            var backgroundRenderer = background.AddComponent<SpriteRenderer>();
            backgroundRenderer.sprite = GetBarSprite();
            backgroundRenderer.color = new Color(0.08f, 0.08f, 0.1f, 0.95f);
            backgroundRenderer.sortingOrder = 20;

            var fill = new GameObject("Fill");
            fillTransform = fill.transform;
            fillTransform.SetParent(root, false);
            fillTransform.localPosition = new Vector3(-width * 0.5f, 0f, 0f);

            fillRenderer = fill.AddComponent<SpriteRenderer>();
            fillRenderer.sprite = GetBarSprite();
            fillRenderer.color = fillColor;
            fillRenderer.sortingOrder = 21;
        }

        private void UpdateBar(float current, float maximum)
        {
            float ratio = maximum > 0f ? Mathf.Clamp01(current / maximum) : 0f;
            fillTransform.localScale = new Vector3(width * ratio, height, 1f);
        }

        private static Sprite GetBarSprite()
        {
            if (barSprite != null)
            {
                return barSprite;
            }

            var texture = new Texture2D(1, 1)
            {
                name = "Runtime Health Bar Texture",
                filterMode = FilterMode.Point,
                hideFlags = HideFlags.HideAndDontSave
            };
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();

            barSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0f, 0.5f), 1f);
            barSprite.name = "Runtime Health Bar Sprite";
            barSprite.hideFlags = HideFlags.HideAndDontSave;
            return barSprite;
        }
    }
}
