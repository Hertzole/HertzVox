using Hertzole.HertzVox.Blocks;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public static class VoxelTerrain
    {
        public static BlockPos GetBlockPos(RaycastHit hit, bool adjacent = false)
        {
            Vector3 pos = new Vector3(MoveWithinBlock(hit.point.x, hit.normal.x, adjacent), MoveWithinBlock(hit.point.y, hit.normal.y, adjacent), MoveWithinBlock(hit.point.z, hit.normal.z, adjacent));

            return pos;
        }

        private static float MoveWithinBlock(float pos, float normal, bool adjacent = false)
        {
            float minHalfBlock = HertzVoxConfig.BlockSize / 2 - 0.01f;
            float maxHalfBlock = HertzVoxConfig.BlockSize / 2 + 0.01f;
            // Because of float imprecision we can't guarantee a hit on the side of a block.

            // Get the distance ofthis potion from the nearest block center accounting for the size of the block.
            float offset = pos - ((int)(pos / 1f) * 1f);
            if ((offset > minHalfBlock && offset < maxHalfBlock) || (offset < -minHalfBlock && offset > -maxHalfBlock))
            {
                if (adjacent)
                    pos += (normal / 2 * HertzVoxConfig.BlockSize);
                else
                    pos -= (normal / 2 * HertzVoxConfig.BlockSize);
            }

            return pos;
        }

        public static Block GetBlock(int x, int y, int z)
        {
            World world = World.Instance;
            Chunk chunk = world.GetChunk(new BlockPos(x, y, z));
            return GetBlock(chunk, new BlockPos(x, y, z));
        }

        public static Block GetBlock(Chunk chunk, int x, int y, int z)
        {
            return GetBlock(chunk, new BlockPos(x, y, z));
        }

        public static Block GetBlock(Chunk chunk, BlockPos pos)
        {
            return chunk.GetBlock(pos);
        }

        public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
        {
            if (hit.transform == null)
                return false;

            Chunk chunk = hit.transform.GetComponent<Chunk>();

            if (chunk == null)
                return false;

            BlockPos pos = GetBlockPos(hit, adjacent);
            chunk.World.SetBlock(pos, block, true);

            return true;
        }

        public static bool SetBlock(BlockPos pos, Block block, World world = null)
        {
            if (!world)
                world = World.Instance;

            if (!world)
                return false;

            Chunk chunk = world.GetChunk(pos);
            if (chunk == null)
                return false;

            chunk.World.SetBlock(pos, block, true);

            return true;
        }

        [System.Obsolete("Not ready to be used!")]
        public static bool FillBlocks(BlockPos startPos, BlockPos endPos, Block block)
        {
            World world = World.Instance;

            if (!world)
                return false;

            world.FillBlocks(startPos, endPos, block);
            return true;
        }
    }
}
