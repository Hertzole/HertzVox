using Hertzole.HertzVox.Blocks;
using UnityEngine;

namespace Hertzole.HertzVox.Examples
{
    public class ExampleWorldGenerator : MonoBehaviour
    {
        [SerializeField]
        private string m_BaseBlockName = "White";
        [SerializeField]
        private string m_GreenBlockName = "Green";

        private World m_World;

        private void Awake()
        {
            m_World = GetComponent<World>();
            //m_World.OnChunkCreated.AddListener(OnGenerateChunk);
            m_World.ChunkCreated += OnGenerateChunk;
        }

        private void OnGenerateChunk(Chunk chunk)
        {
            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                    {
                        Block baseBlock = m_BaseBlockName;
                        Block greenBlock = m_GreenBlockName;

                        if (chunk.Position.y == 0 && y == 0)
                            chunk.SetBlock(new BlockPos(x, y, z), baseBlock, false);
                        else if (((x == 0 && z == 0) || (x == 0 && z == Chunk.CHUNK_SIZE - 1) || (x == Chunk.CHUNK_SIZE - 1 && z == 0) || (x == Chunk.CHUNK_SIZE - 1 && z == Chunk.CHUNK_SIZE - 1)) && y < 5 && chunk.Position.y == 0)
                            chunk.SetBlock(x, y, z, baseBlock, false);
                        else if (((x == 0 && z == 0) || (x == 0 && z == Chunk.CHUNK_SIZE - 1) || (x == Chunk.CHUNK_SIZE - 1 && z == 0) || (x == Chunk.CHUNK_SIZE - 1 && z == Chunk.CHUNK_SIZE - 1)) && y == 5 && chunk.Position.y == 0)
                            chunk.SetBlock(x, y, z, greenBlock, false);
                        else
                            chunk.SetBlock(new BlockPos(x, y, z), "air", false);

                        //Debug.Log("Setting block");

                        //if (chunk.Position.y == 0)
                        //{
                        //    if (y == 0)
                        //        chunk.SetBlock(x, y, z, baseBlock, false);
                        //}

                        //if (chunk.Position.x == 0 && chunk.Position.z == 0)
                        //{
                        //    if (x == 0 && z == 0)
                        //        chunk.SetBlock(x, y, z, greenBlock, false);
                        //}

                        ////Debug.Log((chunk.WorldPosition.x * Chunk.CHUNK_SIZE));
                        //if ((chunk.Position.x / Chunk.CHUNK_SIZE) == m_World.WorldSizeX - 1 && (chunk.Position.z / Chunk.CHUNK_SIZE) == m_World.WorldSizeZ - 1)
                        //{
                        //    if (x == Chunk.CHUNK_SIZE - 1 && z == Chunk.CHUNK_SIZE - 1)
                        //        chunk.SetBlock(x, y, z, greenBlock);
                        //}
                        //else if ((chunk.Position.x / Chunk.CHUNK_SIZE) == 0 && (chunk.Position.z / Chunk.CHUNK_SIZE) == m_World.WorldSizeZ - 1)
                        //{
                        //    if (x == 0 && z == Chunk.CHUNK_SIZE - 1)
                        //        chunk.SetBlock(x, y, z, greenBlock);
                        //}
                        //else if ((chunk.Position.x / Chunk.CHUNK_SIZE) == m_World.WorldSizeX - 1 && (chunk.Position.z / Chunk.CHUNK_SIZE) == 0)
                        //{
                        //    if (x == Chunk.CHUNK_SIZE - 1 && z == 0)
                        //        chunk.SetBlock(x, y, z, greenBlock);
                        //}
                    }
                }
            }

            //Debug.Log("Done generation");
        }
    }
}
