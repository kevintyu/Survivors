using UnityEngine;

namespace Survivors.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 5f;

        private Rigidbody2D body;
        private Vector2 moveInput;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
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
            body.MovePosition(body.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
