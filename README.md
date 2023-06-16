# DBGA - LV1 Assignment
Adventure Game Project DBGA by Maurizio Fischetti

The readme is in the game also.

-Commands: 
Movements: W,A,S,D
Sprint: Left-Shift
Action: E (use it in front of the spawner, yes the sword in the cube)
Attack: F

tips: every time you die you respawn near the.. spawner.. you can die endllessy but you can heal only four time. (The light will let you know)

tip: the dungeon is in the north cavern. to exit access the menu (ESC)
__________________________________________________________

A little Overview:

First of all the game is not complete.
Unfortunately I didn't have the time I hoped to spend time on it, and I'm sorry!

BUT
How is the game done?
In the game there are several systems addressed throughout the course. It's something I wanted to learn, and something else I experimented with.

Here are some:

1- Procedural animations (on my own initiative):
I didn't want to use the animator in its entirety, its only function is to keep track of the animations to use. The rest is managed by a special class inside the AnimationManager.cs
Also there is a small system of attack combos in the player and monsters. It didn't come out on top and I'll have to look into that further.

2- State Machines (discussed in the course):
Experimenting with it was very interesting, unfortunately I only had time to implement a small version of what I wanted to do.
 
3- Interfaces (in the course):
helped me make the mobs state machines and
They have also been very useful for interactive objects (see later)

4- singletons( in the course): I hate them.
 
5- AI (on my own initiative): One of the things that fascinates me the most, I should have managed the movements of the mobs with the navmesh, but for now I'm leaving them stupid and clumsy because it's a topic I'll want to explore, like all things related to AI.

6 -Interaction with objects : unfortunately I only created an object of type "IInteractable" but at the moment it seems to work.

7- Dungeon system (my way): This is the thing I'm most PROUD of! I figured it out myself, and managed to do what I intended to do. Surely there will be ways to improve and optimize it in the future, but basically you can create infinite rooms.

8-Object Pooling(on course): Not optimized yet, but there is a small vague example in the mob spawner.
__________________________________________________________
Known Bugs
See Bug Report file.

__________________________________________________________

Final thougs: 
it is not the end of this project, surely I could have done more if I had more time, moreover it is clear that this thing was not absolutely necessary for now but in reality it has been teaching me to manage time and dedicate myself to the project and to myself. I also learned that I can work even 12 hours a day if something really gets me (even if I will avoid it in the future, better a little and well than a lot and mediocre.).
But I will continue to experiment on this project also for the things I will learn in the future. Surely this project can be the basis for my experiments. But first I have to fix the bugs and the things that don't work.
