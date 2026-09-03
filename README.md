# Survivors Prototype

Open this folder as a Unity project and enter Play mode in any scene. The prototype bootstrap creates a blue player square, a red enemy square, and an orthographic camera automatically.

## Controls

- Move: `WASD` or arrow keys

Movement uses `Rigidbody2D.MovePosition` in `FixedUpdate`, and diagonal input is normalized so it is not faster than horizontal or vertical movement.

## First enemy

The red enemy starts to the right of the player and continually moves toward the player's current position. When the two collide, it deals 10 damage every 0.75 seconds. The player begins with 100 health, while the enemy has 30 health and is destroyed when that reaches zero.

Enemy death is implemented through the public `IDamageable.TakeDamage` contract.

## Automatic weapon

The player automatically fires a yellow projectile at the nearest enemy once per second. Each projectile deals 10 damage, so the prototype enemy dies after three hits. Projectiles are reused through a small object pool rather than repeatedly created and destroyed.

After the enemy dies, a replacement spawns in a random direction from the player following a two-second delay. This is temporary prototype behavior; a data-driven spawn director will replace it in a later milestone.

The movement speed can be adjusted on the `PlayerMovement` component in the Inspector while the game is running. The runtime bootstrap is temporary scaffolding; once a proper gameplay scene and player prefab exist, it can be removed.
