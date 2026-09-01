# Survivors Prototype

Open this folder as a Unity project and enter Play mode in any scene. The prototype bootstrap creates a blue player square and an orthographic camera automatically.

## Controls

- Move: `WASD` or arrow keys

Movement uses `Rigidbody2D.MovePosition` in `FixedUpdate`, and diagonal input is normalized so it is not faster than horizontal or vertical movement.

The movement speed can be adjusted on the `PlayerMovement` component in the Inspector while the game is running. The runtime bootstrap is temporary scaffolding; once a proper gameplay scene and player prefab exist, it can be removed.
