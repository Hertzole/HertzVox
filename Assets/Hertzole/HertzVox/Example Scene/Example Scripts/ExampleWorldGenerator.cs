using UnityEngine;

namespace Hertzole.HertzVox.Examples
{
    public class ExampleWorldGenerator : TerrainGen
    {
        [SerializeField]
        [Range(1, 12)]
        private int m_WallHeight = 7;
        public int WallHeight { get { return m_WallHeight; } set { m_WallHeight = value; } }
        [SerializeField]
        private string m_FloorBlock = "redblock";
        [SerializeField]
        private string m_WallBlock = "whiteconnected";
        [SerializeField]
        private string m_WallTopBlock = "redblock";
        public string WallTopBlock { get { return m_WallTopBlock; } set { m_WallTopBlock = value; } }

        public override void OnGenerateChunkColumn(BlockPos pos)
        {
            Chunk chunk = World.GetChunk(pos);
            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                    {
                        if (pos.y == 0 && y == 0)
                            SetBlock(m_FloorBlock, chunk, x, y, z);
                        else if (((pos.x == (World.WorldMinX * Chunk.CHUNK_SIZE) && x == 0) || (pos.z == (World.WorldMinZ * Chunk.CHUNK_SIZE) && z == 0) || (pos.x == (World.WorldMaxX - 1) * Chunk.CHUNK_SIZE && x == Chunk.CHUNK_SIZE - 1) || (pos.z == (World.WorldMaxZ - 1) * Chunk.CHUNK_SIZE && z == Chunk.CHUNK_SIZE - 1)) && y < m_WallHeight && pos.y < 16)
                            SetBlock(m_WallBlock, chunk, x, y, z);
                        else if (((pos.x == (World.WorldMinX * Chunk.CHUNK_SIZE) && x == 0) || (pos.z == (World.WorldMinZ * Chunk.CHUNK_SIZE) && z == 0) || (pos.x == (World.WorldMaxX - 1) * Chunk.CHUNK_SIZE && x == Chunk.CHUNK_SIZE - 1) || (pos.z == (World.WorldMaxZ - 1) * Chunk.CHUNK_SIZE && z == Chunk.CHUNK_SIZE - 1)) && y >= m_WallHeight && y < m_WallHeight + 1 && pos.y < 16)
                            SetBlock(m_WallTopBlock, chunk, x, y, z);
                        else if (pos.y == 0 && y == 1 && x > 5 && x < 10 && z > 5 && z < 10)
                            SetBlock(m_WallBlock, chunk, x, y, z);
                    }
                }
            }
        }
    }
}
