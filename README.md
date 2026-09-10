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

## Data-driven weapons

Magic Bolt's identity, damage, attack interval, projectile count, speed, lifetime, size, piercing, and pool size are stored in `Assets/Resources/Weapons/MagicBolt.asset`. Runtime weapon components copy values from this shared definition and never modify the asset.

Create additional projectile weapon definitions from `Assets > Create > Survivors > Weapons > Projectile Weapon`. Future area, orbit, and aura weapons will use their own behavior-specific definition types.

## Character stats

`CharacterStats` centrally calculates damage, movement speed, cooldown reduction, projectile speed, projectile count, and maximum health. Movement, weapons, and health read the final calculated values instead of maintaining their own character-wide bonuses.

Runtime modifiers have a source ID and support flat, additive-percent, and multiplicative-percent operations. A future upgrade can replace one modifier or remove every modifier from its source without changing weapon assets.

## Health bars

The player and enemies display world-space health bars above their sprites. The bars subscribe to health-change events and use lightweight sprite renderers rather than creating a UI Canvas for every actor.

## Combat collision

Player and enemy body colliders occupy separate physics layers and do not physically collide, allowing the player to pass through crowds. Child hurtbox triggers receive attacks, enemy contact-hitbox triggers still damage the player, and player projectiles only damage enemy hurtboxes. Body collisions with the Default layer remain enabled for future walls and obstacles.

The movement speed can be adjusted on the `PlayerMovement` component in the Inspector while the game is running. The runtime bootstrap is temporary scaffolding; once a proper gameplay scene and player prefab exist, it can be removed.
