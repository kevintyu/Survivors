using Survivors.Player;
using Survivors.Combat;
using Survivors.Enemies;
using UnityEngine;

namespace Survivors.Core
{
    public static class PrototypeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreatePrototype()
        {
            CreateCameraIfNeeded();

            var player = Object.FindFirstObjectByType<PlayerMovement>();
            if (player == null)
            {
                player = CreatePlayer();
            }

            if (Object.FindFirstObjectByType<EnemyMovement>() == null)
            {
                CreateEnemy(player.transform);
            }
        }

        private static PlayerMovement CreatePlayer()
        {
            var playerObject = new GameObject("Player");
            playerObject.transform.position = Vector3.zero;

            var renderer = playerObject.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateSquareSprite();
            renderer.color = new Color(0.2f, 0.75f, 1f);

            var body = playerObject.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            playerObject.AddComponent<BoxCollider2D>();
            playerObject.AddComponent<Health>().Configure(100f, false);
            return playerObject.AddComponent<PlayerMovement>();
        }

        private static void CreateEnemy(Transform player)
        {
            var enemy = new GameObject("Enemy");
            enemy.transform.position = new Vector3(4f, 0f, 0f);

            var renderer = enemy.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateSquareSprite();
            renderer.color = new Color(1f, 0.25f, 0.25f);

            var body = enemy.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            enemy.AddComponent<BoxCollider2D>();
            enemy.AddComponent<Health>().Configure(30f, true);
            enemy.AddComponent<ContactDamage>().Configure(10f, 0.75f);
            enemy.AddComponent<EnemyMovement>().SetTarget(player);
        }

        private static void CreateCameraIfNeeded()
        {
            if (Camera.main != null)
            {
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = 5f;
                return;
            }

            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.backgroundColor = new Color(0.06f, 0.07f, 0.1f);
        }

        private static Sprite CreateSquareSprite()
        {
            var texture = new Texture2D(1, 1)
            {
                name = "Prototype Square Texture",
                filterMode = FilterMode.Point
            };

            texture.SetPixel(0, 0, Color.white);
            texture.Apply();

            return Sprite.Create(
                texture,
                new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f),
                1f);
        }
    }
}
