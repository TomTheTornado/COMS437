# CollisionGame

Created by Thomas Powell

Com S 437

This is a 3D game which has you control a spaceship to fly through multiple rings.

Currently, you can control the ship with the camera following behind in 3rd person with a skybox around the scene. 
There are rings that form a course, with the next one in the sequence striped, orange ones that you've passed through, gold ones not passed through, and a checkerboard texture for the last ring.
There is a timer that tracks your time and a score that is calculated using your time and rings passed through
The amount of rings that you've missed are displayed as well.

It was made in C# using the Monogame Windows Framework using BepuPhysics 1.5.2. It was written and tested in Visual Studio 2019 on a Windows 7 operating system.
 
__Options for Running The Program:__
1. You can open up SpaceRace.sln in Visual Studio and run it through that.

2. Or you can navigate to Project1/SpaceRace on the command line and enter the command "dotnet run".

3. Or you can navigate to Project1/SpaceRace/bin/Debug/netcoreapp3.1 in the file explorer and run SpaceRace.exe.

__Controls:__
* A/D - roll up and down, respectively
* UP_ARROW/DOWN_ARROW - pitch up and down, respectively
* LEFT_ARROW/RIGHT_ARROW - yaw up and down, respectively
* W/SPACEBAR - thrust forward
* S - thrust backward
* R - restart game
* E - start game
* ESC - quit and exit game


__Attributions:__

This adapts the camera class as well and modifies the asteroid class for use in the rings, skybox, and spaceship from the Asteroid demo on canvas.

Forum that helped with figuring out how to get the camera to follow the ship:
https://gamedev.stackexchange.com/questions/120281/how-do-i-create-a-3rd-person-camera-in-xna

Forum that helped with figuring out text in a 3D monogame project:
https://gamedev.stackexchange.com/questions/31616/spritebatch-begin-making-my-model-not-render-correctly

Model Data Extractor from bepuphysics
https://github.com/bepu/bepuphysics1/blob/master/Documentation/Isolated%20Demos/GettingStartedDemo/ModelDataExtractor.cs

The skybox model and texture were from the canvas course, as well as the ship model and texture.

I created the torus model for the rings in Blender.
I also created the gold, striped, and checkerboard textures for the rings on my own.


