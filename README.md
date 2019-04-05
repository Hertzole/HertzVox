# ARCHIVED!
HertzVox has been archived as I will no longer add new stuff to it, nor fix any problems. I'm unsure if there ever will be a replacement from me, but there are several betters ones out there.

# HertzVox
A Unity voxel framework that is built for Unity and flexibility. This voxel engine does NOT have a "standard". Most engines are based on Minecraft and have the usual infinite terrain, actual "realistic" terrain. But this one, nope! You get the tools to make blocks and some handy events, the rest is up to you. You generate all the blocks/voxels from code.

## Inspiration
I should make it very clear that this is based on both the voxel terrain [tutorial by AlexSTV](http://alexstv.com/index.php/voxelmetric) and their own voxel framework [Voxelmetric](https://github.com/Voxelmetric/Voxelmetric1). It helped me get started, but I felt that their framework got all bloated with the "standard" stuff mentioned above. I just want a voxel framework where I have complete control over each and every chunk it makes.

## Features
### Threading
The engine is working on being multi-threaded so all the chunk updates and such will be called on separate threads and not slowing down the main thread.
### Built in Unity, for Unity
Some voxel frameworks tend to focus on being used by more engines than just Unity. But, this engine will be very narrow-minded and dedicated to Unity. But this also means that everything should work smoothly together with Unity. The game should be smooth and the workflow should be smooth.
### Easily define block types
You can easily add new blocks using Unity's Scriptable Objects. Then you just need to add those blocks to a "Block collection" and lastly reference the block collection in your scripts. You can then get all your blocks from their names. And each and every block supports textures on each side and even connected textures! And the term "block" isn't really that correct either. If you know how to do mesh generation, you could make your own meshes and they could be whatever you like... as long as you can code them.
### Saving and Loading (WIP)
Easily get all the info about the world and the chunks to save it to a file! Everything should work out of the box with every required class being serializable so you can use whatever method of saving you want.
The reason for the WIP is that it is not working as intended with infinite terrains and hasn't been fully tested.
### And more features to come!
I'm trying to actively develop this since I need it for an upcoming game project. And I want it to be the best it can be for that. I'm planning to add more block types, built-in ambient occlusion, and of course, more optimization.

## FAQ
Q: I want to make the next Minecraft. Can I do it with this?  
A: In theory, probably. But it's not meant for that. This is mostly meant for having a fixed world size that you can build upon. You can make terrain generation and such yourself, but the engine does not provide that out of the box.

Q: Why the fixed world size? Why not infinite worlds like all the others?  
A: The personal project I'm working on is not designed to have infinite worlds and I want it to have a fixed world size. And I mean, someone gotta do it, right? Everybody else is doing infinite "realistic" worlds.

Q: How do I do *this thing*?  
A: Check out the example scripts in the example folder. They cover most of the basic stuff and then you should probably be able to figure out the rest.

## Development
As mentioned above, more features will come. I am the only person developing this and it will probably be really messy, but I still want to get it out there for people to maybe enjoy. And as I've mentioned above, I need this for an upcoming game project, so many things will probably adapt around that at first, but will of course change later on to fit more uses.

## License
The license is MIT so I guess you can do whatever you want with the code. I don't really care that much about usage. I just needed somewhere to store my project.
