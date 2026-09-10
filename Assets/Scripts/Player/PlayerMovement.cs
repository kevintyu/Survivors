using Survivors.Stats;
using UnityEngine;

namespace Survivors.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CharacterStats))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D body;
        private CharacterStats stats;
        private Vector2 moveInput;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            stats = GetComponent<CharacterStats>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
        }

        private void Update()
        {
            moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"));

            moveInput = Vector2.ClampMagnitude(moveInput, 1f);
        }

        private void FixedUpdate()
        {
            float moveSpeed = stats.Get(StatType.MoveSpeed);
            body.MovePosition(body.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
