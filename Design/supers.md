## Supers ##
### Super Freeze ###
Aside from having a requirement to use, Supers have the special property have having unique visual effects take place. Below is an outline of game behavior when this takes place

* The performing player will animate normally
	* Optionally the player will have a shadow effect
* The opposing player will
	* Have frozen input until after the freeze.
	* Have frozen animation until after the freeze
* The screen will
	* Fade darker while in freeze OR while the special continues
	* Possibly zoom in on character
	* Other additional sprites ( such as character portrait will appear)

### Current Solution ###
Have a super manager to determine if a super is happening.
A player will have a reference to the same single super manager and notify it when one of them is performing a special.

The super manager will freeze the opponent completely while the players startup actions and so on will continue.

The super manager will also take over the draw stage and then add whichever details are necessary.



### Implementation Details ###
Rules and responsibilities of input manager
 
- Returned moves follow a priority. Special Moves > Command Normals > Normals > Dashes > Movement
- If dashing previously, and the same directional input is pressed, we keep dashing.
- Manager will return move name. Will leave processing of said move to other classes.
- Inputs are stored in an input buffer of variable size. The larger the buffer the more lenient we can be. 
- Traverse the buffer in reverse to see if a move was performed

### Optimizations ###
These are some things we can do to speed things up and make things more efficient. Since this will run every game loop its important this part is performant.

- Only do a full check of input queue on a button press. Dashes can have a stricter time window of checking maybe only 1/2 of input buffer.
- Can keep track of last action name committed.
- On a successful command input ( maybe only specials or something) we can clear the queue. 
	- If we clear just button press we can do  tricks like 236A236A being a special buffered into super.
	- If we clear whole thing much less likely to get accidental commands ( either ones we dont' want or ones players dont' want). 
- Can check to see if done on button press ( on previous game state button is not pressed, on next game state it is pressed) but we can also check to see if button was released for negative edging. 