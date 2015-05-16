## Hitboxes ##

Each move will contain an array of objects that contain the hitbox information for each frame.

Each move will contain information on hitboxes and hurtboxes.

Hurtbox size and such will determine invincibility ( similar to guilty gear). Debugged, the hurtboxes will be blue and hitboxes in red.

A hitbox may not be on every frame as there is obviously start up frames and such.

If a hitbox collides with a hurtbox, then the owner of the hurtbox has been hit.

### Optimizations ###

It's hard to say atm how complex hitboxes can be. Currently for test sakes each frame of a move should have only a single hitbox.

Further optimizations can happen where collision checking isn't done if players aren't close proximities ( though this may be an issue for projectiles)