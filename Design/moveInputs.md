## Reading Inputs ##

### Design Overview ###
Inputs will be passed in to an InputManager. The manager will parse inputs and then return the name of the move done. Will be responsible for determining special commands and dashing. Walking, jumping, and crouching maybe as well

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