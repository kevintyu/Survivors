using System.IO;
using System.Linq;
using Survivors.Weapons.Definitions;
using UnityEditor;
using UnityEngine;

namespace Survivors.Editor
{
    public static class PrototypeContentGenerator
    {
        private const string WeaponsFolder = "Assets/Resources/Weapons";
        private const string VisualPath = WeaponsFolder + "/MagicBoltVisual.asset";
        private const string DefinitionPath = WeaponsFolder + "/MagicBolt.asset";

        [MenuItem("Survivors/Generate Prototype Content")]
        public static void Generate()
        {
            Directory.CreateDirectory(WeaponsFolder);
            Sprite projectileSprite = LoadOrCreateProjectileSprite();
            var definition = AssetDatabase.LoadAssetAtPath<ProjectileWeaponDefinition>(DefinitionPath);

            if (definition == null)
            {
                definition = ScriptableObject.CreateInstance<ProjectileWeaponDefinition>();
                definition.ConfigureBase(
                    "magic-bolt",
                    "Magic Bolt",
                    "Automatically fires a bolt at the nearest enemy.",
                    projectileSprite,
                    10f,
                    1f,
                    1);
                definition.ConfigureProjectile(projectileSprite, 8f, 3f, 0.25f, 0, 8);
                AssetDatabase.CreateAsset(definition, DefinitionPath);
            }

            EditorUtility.SetDirty(definition);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var savedDefinition = AssetDatabase.LoadAssetAtPath<ProjectileWeaponDefinition>(DefinitionPath);
            string error = "the asset could not be loaded";
            if (savedDefinition == null || !savedDefinition.IsValid(out error))
            {
                throw new InvalidDataException($"Generated Magic Bolt definition is invalid: {error}");
            }

            Debug.Log($"Prototype weapon content is ready at {DefinitionPath}.");
        }

        private static Sprite LoadOrCreateProjectileSprite()
        {
            Sprite existing = AssetDatabase.LoadAllAssetsAtPath(VisualPath).OfType<Sprite>().FirstOrDefault();
            if (existing != null)
            {
                return existing;
            }

            var texture = new Texture2D(1, 1)
            {
                name = "Magic Bolt Texture",
                filterMode = FilterMode.Point
            };
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            AssetDatabase.CreateAsset(texture, VisualPath);

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f), 1f);
            sprite.name = "Magic Bolt Sprite";
            AssetDatabase.AddObjectToAsset(sprite, texture);
            return sprite;
        }
    }
}
