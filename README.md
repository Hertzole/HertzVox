# HertzVox
A Unity voxel framework that is built for Unity and flexibility. This voxel engine does NOT have a "standard". Most engines are based on Minecraft and have the usual infinite terrain, actual "realistic" terrain. But this one, nope! You get the tools to make blocks and some handy events, the rest is up to you. You generate all the blocks/voxels from code.

## Inspiration
I should make it very clear that this is based on both the voxel terrain [tutorial by AlexSTV](http://alexstv.com/index.php/voxelmetric) and their own voxel framework [Voxelmetric](https://github.com/Voxelmetric/Voxelmetric1). It helped me get started, but I felt that their framework got all bloated with the "standard" stuff mentioned above. I just want a voxel framework where I have complete control over each and every chunk it makes.

## Features
### Threading
The engine is working on being multi-threaded so all the chunk updates and such will be called on separate threads and thus no clog up the main thread, making the game stutter.
### Built in Unity, for Unity
Some voxel frameworks tend to focus on being used by more engines than just Unity. But, this engine will be very narrow-minded and dedicated to Unity. But this also means that everything should work smoothly together with Unity. The game should be smooth and the workflow should be smooth.
### Easily define block types
You can easily add new blocks using Unity's Scriptable Objects. Then you just need to add those blocks to a "Block collection" and lastly reference the block collection in your scripts. You can then get all your blocks from their names. And each and every block supports textures on each side and even connected textures! And the term "block" isn't really that correct either. If you know how to do mesh generation, you could make your own meshes and they could be whatever you like... as long as you can code them.
### And more features to come!
I'm trying to actively develop this since I need it for an upcoming game project. And I want it to be the best it can be for that. Some upcoming features I'm planning are saving & loading, more block types and built-in ambient occlusion.

## Development
As mentioned above, more features will come. I am the only person developing this and it will probably be really messy, but I still want to get it out there for people to maybe enjoy. And as I've mentioned above, I need this for an upcoming game project, so many things will probably adapt around that at first, but will of course change later on to fit more uses.

## License
The license is MIT so I guess you can do whatever you want with the code as long as you credit me as the original author. ¯\_(ツ)_/¯
