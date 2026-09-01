using Survivors.Player;
using UnityEngine;

namespace Survivors.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class EnemyMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 2f;

        private Rigidbody2D body;
        private Transform target;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
        }

        private void Start()
        {
            if (target == null)
            {
                var player = Object.FindFirstObjectByType<PlayerMovement>();
                target = player != null ? player.transform : null;
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void FixedUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector2 direction = ((Vector2)target.position - body.position).normalized;
            body.MovePosition(body.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
