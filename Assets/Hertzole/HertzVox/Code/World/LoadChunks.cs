using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public class LoadChunks : MonoBehaviour
    {
        World World { get { return World.Instance; } }

        private int m_DeleteTimer = 0;
        private int m_ChunkGenTimer = 0;

        private int m_WorldMaxY = 64;
        private int WorldMaxY { get { return World.WorldSizeY * Chunk.CHUNK_SIZE; } }
        private int m_WorldMinY = 0;
        private int WorldMinY { get { return 0; } }

        private const int WAIT_BETWEEN_DELETES = 10;
        private const int WAIT_BETWEN_CHUNK_GEN = 1;

        private void Start()
        {
            World.CreateWorld();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_DeleteTimer == WAIT_BETWEEN_DELETES)
            {
                DeleteChunks();
                m_DeleteTimer = 0;
                return;
            }
            else
            {
                m_DeleteTimer++;
            }

            if (m_ChunkGenTimer == WAIT_BETWEN_CHUNK_GEN)
            {
                FindChunksAndLoad();
                m_ChunkGenTimer = 0;
                return;
            }
            else
            {
                m_ChunkGenTimer++;
            }
        }

        void DeleteChunks()
        {
            var chunksToDelete = new List<BlockPos>();
            foreach (var chunk in World.Chunks)
            {
                Vector3 chunkPos = chunk.Key;
                float distance = Vector3.Distance(
                    new Vector3(chunkPos.x, 0, chunkPos.z),
                    new Vector3(transform.position.x, 0, transform.position.z));

                if (distance > HertzVoxConfig.DISTANCE_TO_DELETE_CHUNKS * HertzVoxConfig.BlockSize)
                    chunksToDelete.Add(chunk.Key);
            }

            foreach (var chunk in chunksToDelete)
            {
                Chunk _chunk = World.GetChunk(chunk);
                _chunk.gameObject.SetActive(false);
                _chunk.SetFlag(Chunk.Flag.Loaded, false);
            }
        }

        private bool FindChunksAndLoad()
        {
            // Cycle through the array of positions
            for (int i = 0; i < Data.ChunkLoadOrder.Length; i++)
            {
                // Get the position of this gameobject to generate around
                BlockPos playerPos = ((BlockPos)transform.position).ContainingChunkCoordinates();

                // Translate the player position and array position into chunk position
                BlockPos newChunkPos = new BlockPos(Data.ChunkLoadOrder[i].x * Chunk.CHUNK_SIZE + playerPos.x, 0, Data.ChunkLoadOrder[i].z * Chunk.CHUNK_SIZE + playerPos.z);

                // Get the chunk in the defined position
                Chunk newChunk = World.GetChunk(newChunkPos);

                // If the chunk already exists and it's already
                // rendered or in queue to be rendered continue
                if (newChunk != null && newChunk.GetFlag(Chunk.Flag.Loaded))
                    continue;
                else if (newChunk == null) // Or if the chunk is null (out of bounds)
                    continue;

                LoadChunkColumn(newChunkPos);
                return true;
            }

            return false;
        }

        public void LoadChunkColumn(BlockPos columnPosition)
        {
            for (int y = WorldMaxY; y >= WorldMinY; y -= Chunk.CHUNK_SIZE)
            {
                BlockPos pos = new BlockPos(columnPosition.x, y, columnPosition.z);
                Chunk chunk = World.GetChunk(pos);
                if (chunk != null)
                {
                    chunk.SetFlag(Chunk.Flag.Loaded, true);
                    chunk.gameObject.SetActive(true);
                }
            }

            // Start the threaded chunk generation
            if (HertzVoxConfig.UseMultiThreading)
            {
                Thread thread = new Thread(() => { LoadChunkColumnInner(columnPosition); });
                thread.Start();
            }
            else
            {
                LoadChunkColumnInner(columnPosition);
            }

        }

        void LoadChunkColumnInner(BlockPos columnPosition)
        {
            Chunk chunk;
            // Terrain generation can happen in another thread meaning that we will reach this point before the
            // thread completes, we need to wait for all the chunks we depend on to finish generating before we
            // can calculate any light spread or render the chunk
            if (HertzVoxConfig.UseMultiThreading)
            {
                for (int y = WorldMaxY; y >= WorldMinY; y -= Chunk.CHUNK_SIZE)
                {
                    for (int x = -Chunk.CHUNK_SIZE; x <= Chunk.CHUNK_SIZE; x += Chunk.CHUNK_SIZE)
                    {
                        for (int z = -Chunk.CHUNK_SIZE; z <= Chunk.CHUNK_SIZE; z += Chunk.CHUNK_SIZE)
                        {
                            chunk = World.GetChunk(columnPosition.Add(x, y, z));
                            if (chunk)
                            {
                                while (!chunk.GetFlag(Chunk.Flag.TerrainGenerated))
                                    Thread.Sleep(0);
                            }
                        }
                    }
                }
            }

            // Render chunk
            for (int y = WorldMaxY; y >= WorldMinY; y -= Chunk.CHUNK_SIZE)
            {
                chunk = World.GetChunk(columnPosition.Add(0, y, 0));

                if (chunk)
                    chunk.UpdateChunk();
            }
        }

    }
}
