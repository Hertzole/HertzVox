using Hertzole.HertzVox.Blocks;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public class TerrainGen : MonoBehaviour
    {
        private World m_World;
        public World World { get { return m_World; } set { m_World = value; } }

        public virtual void OnGenerateChunkColumn(BlockPos pos) { }

        public void SetBlock(Block block, Chunk chunk, int x, int y, int z)
        {
            SetBlock(block, chunk, new BlockPos(x, y, z));
        }

        public void SetBlock(Block block, Chunk chunk, BlockPos pos)
        {
            if (chunk == null)
                return;

            block.Modified = false;
            chunk.SetBlock(pos, block, false);
        }
    }
}
