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
            //TODO: Move '1' (block size) into config.
            float minHalfBlock = 1f / 2 - 0.01f;
            float maxHalfBlock = 1f / 2 + 0.01f;
            // Because of float imprecision we can't guarantee a hit on the side of a block.

            // Get the distance ofthis potion from the nearest block center accounting for the size of the block.
            float offset = pos - ((int)(pos / 1f) * 1f);
            if ((offset > minHalfBlock && offset < maxHalfBlock) || (offset < -minHalfBlock && offset > -maxHalfBlock))
            {
                if (adjacent)
                    pos += (normal / 2 * 1f);
                else
                    pos -= (normal / 2 * 1f);
            }

            return pos;
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

            //TODO: Lighting

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

            //TODO: Lighting

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
