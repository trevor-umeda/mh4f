## Attacking ##

Moves will be denoted with an IsAttack bool to signify it is an attack, which separates it from other animations.
Holding away from the attacker will block

Types of moves are as follows

* A normal is an attack that requires just a button press.
* A command normal (Not yet a thing) involves a directional input and an attack button press.
* A special move takes a sequence of moves and ending in a button press

Inputting an attack move has certain ramifications for the player.

* Upon inputting an attack. The player is restricted from other attacks unless his attack connects with opponent
* Upon hitting, blocking or not, the player may cancel that move into another normal or special. Which normals and specials are allowed may be specified in config and in the move object.
* The same goes with movements in terms of cancels
* A move that has been canceled into may have different properties. May start at a different frame or something
* A certain amount of pushback must be calculated on the move that will effect the attacking player

A defending player will be hit or be blocking once the opponents hitbox collides with his hurtbox. What happens to the player will calculated by that players object. 
A player that is blocking is under certain rules

* They are in block stun
* They don't take damage

A player that has been hit is under certain rules

* They take damage depending on the attack taken
* They are under a certain amount of hitstun
* They cannot do anything (atm) while being hit and in hitstun