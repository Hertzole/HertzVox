using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public static class Data
    {
        private static BlockPos[] chunkLoadOrder;
        public static BlockPos[] ChunkLoadOrder { get { return chunkLoadOrder; } }

        static Data()
        {
            var chunkLoads = new List<BlockPos>();
            for (int x = -HertzVoxConfig.CHUNK_LOAD_RADIUS; x <= HertzVoxConfig.CHUNK_LOAD_RADIUS; x++)
            {
                for (int z = -HertzVoxConfig.CHUNK_LOAD_RADIUS; z <= HertzVoxConfig.CHUNK_LOAD_RADIUS; z++)
                {
                    chunkLoads.Add(new BlockPos(x, 0, z));
                }
            }

            // limit how far away the blocks can be to achieve a circular loading pattern
            float maxRadius = HertzVoxConfig.CHUNK_LOAD_RADIUS * 1.55f;

            //sort 2d vectors by closeness to center
            chunkLoadOrder = chunkLoads
                                .Where(pos => Mathf.Abs(pos.X) + Mathf.Abs(pos.Z) < maxRadius)
                                .OrderBy(pos => Mathf.Abs(pos.X) + Mathf.Abs(pos.Z)) //smallest magnitude vectors first
                                .ThenBy(pos => Mathf.Abs(pos.X)) //make sure not to process e.g (-10,0) before (5,5)
                                .ThenBy(pos => Mathf.Abs(pos.Z))
                                .ToArray();
        }
    }
}
