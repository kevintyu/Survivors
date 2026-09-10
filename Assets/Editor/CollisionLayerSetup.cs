using System;
using System.Linq;
using Survivors.Combat;
using UnityEditor;
using UnityEngine;

namespace Survivors.Editor
{
    public static class CollisionLayerSetup
    {
        private const int PlayerLayer = 6;
        private const int EnemyLayer = 7;
        private const int PlayerProjectileLayer = 8;
        private const int EnemyContactLayer = 9;

        [MenuItem("Survivors/Configure Combat Layers")]
        public static void Apply()
        {
            UnityEngine.Object tagManagerAsset =
                AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset").First();
            var tagManager = new SerializedObject(tagManagerAsset);
            SerializedProperty layers = tagManager.FindProperty("layers");

            SetLayer(layers, PlayerLayer, CombatLayers.PlayerName);
            SetLayer(layers, EnemyLayer, CombatLayers.EnemyName);
            SetLayer(layers, PlayerProjectileLayer, CombatLayers.PlayerProjectileName);
            SetLayer(layers, EnemyContactLayer, CombatLayers.EnemyContactName);
            tagManager.ApplyModifiedPropertiesWithoutUndo();

            CombatLayers.ApplyCollisionRules();
            UnityEngine.Object physicsSettings =
                AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Physics2DSettings.asset").First();
            EditorUtility.SetDirty(tagManagerAsset);
            EditorUtility.SetDirty(physicsSettings);
            AssetDatabase.SaveAssets();

            Validate();
            Debug.Log("Combat layers and 2D collision rules configured successfully.");
        }

        private static void SetLayer(SerializedProperty layers, int index, string layerName)
        {
            SerializedProperty layer = layers.GetArrayElementAtIndex(index);
            if (!string.IsNullOrEmpty(layer.stringValue) && layer.stringValue != layerName)
            {
                throw new InvalidOperationException(
                    $"Layer {index} is already assigned to '{layer.stringValue}'.");
            }

            layer.stringValue = layerName;
        }

        private static void Validate()
        {
            Require(LayerMask.NameToLayer(CombatLayers.PlayerName) == PlayerLayer,
                "Player layer was not saved.");
            Require(LayerMask.NameToLayer(CombatLayers.EnemyName) == EnemyLayer,
                "Enemy layer was not saved.");
            Require(LayerMask.NameToLayer(CombatLayers.PlayerProjectileName) == PlayerProjectileLayer,
                "PlayerProjectile layer was not saved.");
            Require(LayerMask.NameToLayer(CombatLayers.EnemyContactName) == EnemyContactLayer,
                "EnemyContact layer was not saved.");

            Require(Physics2D.GetIgnoreLayerCollision(PlayerLayer, EnemyLayer),
                "Player and enemy bodies still collide.");
            Require(Physics2D.GetIgnoreLayerCollision(EnemyLayer, EnemyLayer),
                "Enemy bodies still collide with one another.");
            Require(Physics2D.GetIgnoreLayerCollision(PlayerProjectileLayer, PlayerLayer),
                "Player projectiles still collide with the player layer.");
            Require(!Physics2D.GetIgnoreLayerCollision(PlayerProjectileLayer, EnemyLayer),
                "Player projectiles cannot reach enemy hurtboxes.");
            Require(!Physics2D.GetIgnoreLayerCollision(EnemyContactLayer, PlayerLayer),
                "Enemy contact hitboxes cannot reach the player hurtbox.");
            Require(Physics2D.GetIgnoreLayerCollision(EnemyContactLayer, EnemyLayer),
                "Enemy contact hitboxes still collide with enemy bodies.");
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
