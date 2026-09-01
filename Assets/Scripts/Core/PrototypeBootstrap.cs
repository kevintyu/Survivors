using Survivors.Player;
using UnityEngine;

namespace Survivors.Core
{
    public static class PrototypeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreatePrototype()
        {
            CreateCameraIfNeeded();

            if (Object.FindFirstObjectByType<PlayerMovement>() != null)
            {
                return;
            }

            var player = new GameObject("Player");
            player.transform.position = Vector3.zero;

            var renderer = player.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateSquareSprite();
            renderer.color = new Color(0.2f, 0.75f, 1f);

            var body = player.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            player.AddComponent<BoxCollider2D>();
            player.AddComponent<PlayerMovement>();
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
                name = "Prototype Player Texture",
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
