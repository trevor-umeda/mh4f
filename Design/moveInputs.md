## Reading Inputs ##

### Design Overview ###
Inputs will be passed in to a pair or collection of objects that will store, manage, and parse inputs. The manager will parse inputs and then return the name of the move done and that is all. It is not up to these classes to determine the validity of the special input, only what was inputted. Several optimizations are in place though to prevent needless checking and parsing

### Current Solution ###
The input manager will contain a queue of keyboard states. Its a specially modified one as it only tracks button presses and not held down buttons ( which could be a mistake).

Basic movements that are universal, such as walking and jumping, are handled outside of the input manager and by the player itself.

If an attack button is pressed then the InputManager runs through its tracked inputs and sees if if any registered special command was put in. It travels past to present through the queue, and the first special input to have been registered as "inputted" will be used.

Special movement inputs such as dashing/backdashing are special universal inputs and cannot be changed. This looks through the tracked inputs from present to past. to have the most responsive dashing and does not need to worry about move priority.

The input manager trickles the name of the move upwards .

### Algorithm Ideas ###

### Idea 1 ###
Store every input in our input buffer. Traverse it backwards for speed so on a attack button press we can determine if it matches a special command input as soon as possible and exit the search loop. However if we traverse it forwards we can determine move priority much easier. However that would mean a full search no matter what. 

As an optimization we can only do a full search once a button is pressed down. If no button is pressed we shouldn't have to look for a special move. Parsing dashes will be separate.

Atm going forward doesn't seem too expensive so I'll continue on it for now.

#### Idea 2 ####
Don't store a new input if its the same as the previous input. These would keep the input buffer small and the amount of inputs you'd need to read in could be kept small. With a smaller queue we can process it reading forward rather than backwards which would leave priorities of different inputs to be much easier to implement.

The one issue is if it only stores inputs of on detection of a new command inputted, then the queue would never grow stale. A potential workaround is to then store amount of repeated frames an input has to essentially track time. Then if the special command takes too many frames then we can then ignore the input 


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