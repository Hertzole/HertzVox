using Hertzole.HertzVox.Blocks;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzVox.Saving
{
    public static class VoxSaveManager
    {
        public static VoxSave GetSaveData(World world = null)
        {
            if (world == null)
            {
                world = World.Instance;

                if (world == null)
                {
                    Debug.LogError("There's no world in the scene when attempting to save!");
                    return null;
                }
            }

            List<Chunk> chunksToSave = new List<Chunk>();
            chunksToSave.AddRange(world.Chunks.Values);
            VoxSave save = new VoxSave();

            for (int i = 0; i < chunksToSave.Count; i++)
            {
                save.chunks.Add(new VoxSaveChunk(chunksToSave[i]));
            }

            return save;
        }

        public static void ApplySaveData(VoxSave data, World world = null)
        {
            if (world == null)
            {
                world = World.Instance;

                if (world == null)
                {
                    Debug.LogError("There's no world to apply save data to!");
                    return;
                }
            }

            for (int i = 0; i < data.chunks.Count; i++)
            {
                Chunk chunk = world.GetChunk(data.chunks[i].chunkPosition);
                for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
                {
                    for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
                    {
                        for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                        {
                            chunk.SetBlock(x, y, z, Block.Air, false);
                        }
                    }
                }

                for (int b = 0; b < data.chunks[i].blocks.Length; b++)
                {
                    chunk.SetBlock(data.chunks[i].positions[b], data.chunks[i].blocks[b], false);
                }

                chunk.SetFlag(Chunk.Flag.Loaded, false);
            }
        }
    }
}
