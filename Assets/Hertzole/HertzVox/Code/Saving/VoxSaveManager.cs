using Hertzole.HertzVox.Blocks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Hertzole.HertzVox.Saving
{
    public static class VoxSaveManager
    {
        public static VoxSave GetSaveData(World world = null, bool saveAll = false)
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
                VoxSaveChunk saveChunk = new VoxSaveChunk(chunksToSave[i], saveAll);
                if (saveChunk.changed)
                    save.chunks.Add(saveChunk);
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

            if (HertzVoxConfig.UseMultiThreading)
            {
                Thread thread = new Thread(() =>
                {
                    for (int i = 0; i < data.chunks.Count; i++)
                    {
                        Chunk chunk = world.GetChunk(data.chunks[i].chunkPosition);
                        if (chunk)
                        {
                            for (int b = 0; b < data.chunks[i].blocks.Length; b++)
                            {
                                Block blockToPlace = data.chunks[i].blocks[b];
                                blockToPlace.Modified = false;
                                chunk.SetBlock(data.chunks[i].positions[b], blockToPlace, false);
                            }

                            chunk.SetFlag(Chunk.Flag.Loaded, false);
                        }
                    }
                });
                thread.Start();
            }
        }
    }
}
