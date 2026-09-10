using System;
using UnityEngine;

namespace Survivors.Combat
{
    public static class CombatLayers
    {
        public const string PlayerName = "Player";
        public const string EnemyName = "Enemy";
        public const string PlayerProjectileName = "PlayerProjectile";
        public const string EnemyContactName = "EnemyContact";

        public static int Player => GetRequired(PlayerName);
        public static int Enemy => GetRequired(EnemyName);
        public static int PlayerProjectile => GetRequired(PlayerProjectileName);
        public static int EnemyContact => GetRequired(EnemyContactName);

        public static void ApplyCollisionRules()
        {
            Physics2D.IgnoreLayerCollision(Player, Enemy, true);
            Physics2D.IgnoreLayerCollision(Enemy, Enemy, true);
            Physics2D.IgnoreLayerCollision(PlayerProjectile, Player, true);
            Physics2D.IgnoreLayerCollision(PlayerProjectile, EnemyContact, true);
            Physics2D.IgnoreLayerCollision(EnemyContact, Enemy, true);
        }

        private static int GetRequired(string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (layer < 0)
            {
                throw new InvalidOperationException(
                    $"Required physics layer '{layerName}' has not been configured.");
            }

            return layer;
        }
    }
}
