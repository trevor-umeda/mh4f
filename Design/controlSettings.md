## Control Setting ##

It is necessary, or at least would be pretty bootleg, if you could not change your controls. This means that controls cannot be hardcoded to Keys values. 

There is a ControlSettings object that each player has that will keep track of the current control scheme for that player.

Due to there being numerous objects that use player input, these numerous objects have to, in some way, keep track of this ControlSettings object.

### Current Solution ###
The current solution is the all objects will have reference to this ControlSettings object and the dependency is injected.

### Previous Solutions ###
First attempts were to consolidate all input reading into a single class, but this broke the functionality of most of the classes and made the code alot uglier.

The second solution was to have only player keep hold of control setting and every method passed along control setting, which was also ugly and made method signatures unnatural looking

