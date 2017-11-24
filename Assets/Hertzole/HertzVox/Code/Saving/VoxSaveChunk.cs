using Hertzole.HertzVox.Blocks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzVox.Saving
{
    [Serializable]
    public class VoxSaveChunk
    {
        public BlockPos chunkPosition;
        public BlockPos[] positions = new BlockPos[0];
        public Block[] blocks = new Block[0];

        public VoxSaveChunk(Chunk chunk)
        {
            try
            {
                chunkPosition = chunk.Position;

                Dictionary<BlockPos, Block> blockDictionary = new Dictionary<BlockPos, Block>();

                for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
                {
                    for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
                    {
                        for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                        {
                            BlockPos pos = new BlockPos(x, y, z);
                            if (chunk.GetBlock(pos).type != Block.Air.type)
                            {
                                blockDictionary.Remove(pos);
                                blockDictionary.Add(pos, chunk.GetBlock(pos));
                            }
                        }
                    }
                }

                blocks = new Block[blockDictionary.Keys.Count];
                positions = new BlockPos[blockDictionary.Keys.Count];

                int index = 0;
                foreach (var pair in blockDictionary)
                {
                    blocks[index] = pair.Value;
                    positions[index] = pair.Key;
                    index++;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}
