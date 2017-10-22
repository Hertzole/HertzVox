using Hertzole.HertzVox.Blocks;
using System;

namespace Hertzole.HertzVox
{
    public class BlockOverride
    {
        public BlockController m_Controller;

        public virtual Block OnCreate(Chunk chunk, BlockPos pos, Block block)
        {
            return block;
        }

        public virtual void OnDestroy(Chunk chunk, BlockPos pos, Block block) { }

        public virtual void PreRender(Chunk chunk, BlockPos pos, Block block) { }

        public virtual void PostRender(Chunk chunk, BlockPos pos, Block block) { }

        public virtual void RandomUpdate(Chunk chunk, BlockPos pos, Block block) { }

        public virtual void ScheduledUpdate(Chunk chunk, BlockPos pos, Block block) { }

        public static BlockOverride GetBlockOverride(int blockType)
        {
            return Block.Index.blockOverrides[blockType];
        }

        public virtual Object GetFlagIntercept(Object key, Chunk chunk, BlockPos pos, Block block)
        {
            return null;
        }
    }
}
