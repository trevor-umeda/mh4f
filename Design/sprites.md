### Sprites and Moves ###

This is from the technical side. Artistic guidelines are not considered here.

### A Move ###
A player will have a collection of moves. Each move will have a name which the player object uses to make that move the Current Move. A move has its own sprite animation, which is a single image containing several sprites. Animation of a sprite is simply alerting which part of the image is being rendered. 

Additionally and not related to sprite rendering is each move responsible for containing information pertaining to itself.

Some examples include:

* Hitbox and hurtbox info
* Damage
* The sprite
* Blockstun, Hitstun and Pushback
* The specific state it puts a player in

The above is not a complete list