using Hertzole.HertzVox.Blocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace Hertzole.HertzVox
{
    public class World : MonoBehaviour
    {
        [System.Obsolete("Use 'TerrainGen' method instead.")]
        public class ChunkEvent : UnityEvent<Chunk> { }

        [SerializeField]
        private Chunk m_ChunkPrefab;
        public Chunk ChunkPrefab { get { return m_ChunkPrefab; } set { m_ChunkPrefab = value; } }
        [SerializeField]
        private BlockCollection m_BlockCollection;
        public BlockCollection BlockCollection { get { return m_BlockCollection; } set { m_BlockCollection = value; } }

        [Header("World Settings")]
        [SerializeField]
        [Range(1, 16)]
        private int m_WorldSizeX = 4;
        public int WorldSizeX { get { return m_WorldSizeX; } set { m_WorldSizeX = value; } }
        [SerializeField]
        [Range(1, 16)]
        private int m_WorldSizeY = 2;
        public int WorldSizeY { get { return m_WorldSizeY; } set { m_WorldSizeY = value; } }
        [SerializeField]
        [Range(1, 16)]
        private int m_WorldSizeZ = 4;
        public int WorldSizeZ { get { return m_WorldSizeZ; } set { m_WorldSizeZ = value; } }

        // All the chunks in the world.
        private Dictionary<BlockPos, Chunk> m_Chunks = new Dictionary<BlockPos, Chunk>();
        /// <summary> All the chunks in the world. </summary>
        public Dictionary<BlockPos, Chunk> Chunks { get { return m_Chunks; } set { m_Chunks = value; } }
        // All the chunks that will be updated on a fill block operation.
        private Dictionary<BlockPos, Chunk> m_ChunksToUpdate = new Dictionary<BlockPos, Chunk>();

        private List<Chunk> m_ChunkPool = new List<Chunk>();

        private static World instance;
        public static World Instance { get { if (!instance) instance = FindObjectOfType<World>(); return instance; } }

        private TerrainGen m_TerrainGen;

        private System.Random m_Random;
        public System.Random Random { get { return m_Random; } set { m_Random = value; } }

        [System.Obsolete("Make use of 'TerrainGen' instead.")]
        private ChunkEvent m_OnChunkCreated = new ChunkEvent();
        [System.Obsolete("Make use of 'TerrainGen' instead.")]
        public ChunkEvent OnChunkCreated { get { return m_OnChunkCreated; } set { m_OnChunkCreated = value; } }
        public delegate void ChunkCreateEvent(Chunk chunk);
        [System.Obsolete("Make use of 'TerrainGen' instead.")]
        public event ChunkCreateEvent ChunkCreated;

        private void Awake()
        {
            Block.Index.GetMissingDefinitions(BlockCollection);

            m_Random = new System.Random();

            m_TerrainGen = GetComponent<TerrainGen>();

            if (m_TerrainGen == null)
                Debug.LogError("There's no component based on 'TerrainGen' attached to '" + gameObject.name + "'! Chunks will not be generated.");
            else
                m_TerrainGen.World = this;
        }

        public void CreateWorld()
        {
            for (int i = Chunks.Count - 1; i >= 0; i--)
            {
                AddToChunkPool(Chunks.ElementAt(i).Value);
                Chunks.Remove(Chunks.ElementAt(i).Key);
            }

            for (int x = 0; x < WorldSizeX; x++)
            {
                for (int y = 0; y < WorldSizeY; y++)
                {
                    for (int z = 0; z < WorldSizeZ; z++)
                    {
                        CreateChunk(new BlockPos(x * Chunk.CHUNK_SIZE, y * Chunk.CHUNK_SIZE, z * Chunk.CHUNK_SIZE));
                    }
                }
            }
        }

        /// <summary>
        /// Instantiates a chunk at the supplied coordinates using the chunk prefab,
        /// then runs terrain generation on it and loads the chunk's save file
        /// </summary>
        /// <param name="pos">The world position to create this chunk.</param>
        public void CreateChunk(BlockPos pos)
        {
            Chunk newChunkObject;
            if (m_ChunkPool.Count == 0)
            {
                // No chunks in pool, create new
                newChunkObject = Instantiate(ChunkPrefab, pos, Quaternion.Euler(Vector3.zero));
            }
            else
            {
                // Load a chunk from the pool
                newChunkObject = m_ChunkPool[0];
                m_ChunkPool.RemoveAt(0);
                newChunkObject.gameObject.SetActive(true);
                newChunkObject.transform.position = pos;
            }

            newChunkObject.transform.parent = gameObject.transform;
            newChunkObject.transform.name = "Chunk (" + pos + ")";

            newChunkObject.Position = pos;
            newChunkObject.World = this;

            // Add it to the chunks dictionary with the position as the key
            m_Chunks.Add(pos, newChunkObject);

            if (HertzVoxConfig.UseMultiThreading)
            {
                Thread thread = new Thread(() => { GenAndLoadChunk(newChunkObject); });
                thread.Start();
            }
            else
            {
                GenAndLoadChunk(newChunkObject);
            }
        }

        /// <summary>
        /// Load terrain, saved changes and resets
        /// the light for an empty chunk
        /// </summary>
        /// <param name="chunk">The chunk to generate and load for</param>
        protected virtual void GenAndLoadChunk(Chunk chunk)
        {
            try
            {
                m_TerrainGen.OnGenerateChunkColumn(chunk.Position);

                chunk.SetFlag(Chunk.Flag.TerrainGenerated, true);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Saves the chunk and destroys the game object
        /// </summary>
        /// <param name="pos">Position of the chunk to destroy</param>
        public void DestroyChunk(BlockPos pos)
        {
            Chunk chunk = null;
            if (Chunks.TryGetValue(pos, out chunk))
            {
                if (HertzVoxConfig.UseMultiThreading)
                {
                    Thread thread = new Thread(() =>
                    {
                        chunk.MarkForDeletion();
                    });
                    thread.Start();
                }
                else
                {
                    chunk.MarkForDeletion();
                }

                Chunks.Remove(pos);
            }
        }

        public void AddToChunkPool(Chunk chunk)
        {
            chunk.gameObject.SetActive(false);
            m_ChunkPool.Add(chunk);
        }

        /// <summary>
        /// Get's the chunk object at pos
        /// </summary>
        /// <param name="pos">Position of the chunk or of a block within the chunk</param>
        /// <returns>chunk that contains the given block position or null if there is none</returns>
        public Chunk GetChunk(BlockPos pos)
        {
            pos = pos.ContainingChunkCoordinates();

            Chunk containerChunk = null;
            Chunks.TryGetValue(pos, out containerChunk);

            return containerChunk;
        }

        /// <summary>
        /// Gets the block at pos
        /// </summary>
        /// <param name="pos">Global position of the block</param>
        /// <returns>The block at the given global coordinates</returns>
        public Block GetBlock(BlockPos pos)
        {
            Chunk containerChunk = GetChunk(pos);

            if (containerChunk != null)
            {
                BlockPos localPos = pos - containerChunk.Position;
                if (!Chunk.InRange(localPos))
                {
                    Debug.LogError("Error while setting block");
                    return Block.Void;
                }

                return containerChunk.GetBlock(localPos);
            }
            else
            {
                return "solid";
            }
        }

        /// <summary>
        /// Gets the chunk and sets the block at the given coordinates, updates the chunk and its
        /// neighbors if the update chunk flag is true or not set. Uses global coordinates, to use
        /// local coordinates use the chunk's SetBlock function.
        /// </summary>
        /// <param name="pos">Global position of the block</param>
        /// <param name="block">The block be placed</param>
        /// <param name="updateChunk">Optional parameter, set to false not update the chunk despite the change</param>
        public void SetBlock(BlockPos pos, Block block, bool updateChunk = true)
        {
            Chunk chunk = GetChunk(pos);
            if (chunk != null)
            {
                BlockPos localPos = pos - chunk.Position;
                chunk.SetBlock(localPos, block, updateChunk);

                if (updateChunk)
                    UpdateAdjacentChunks(pos);
            }
            //else
            //Debug.LogError("No chunk at " + pos);
        }

        [System.Obsolete("Not ready to be used!")]
        public void FillBlocks(BlockPos startPos, BlockPos endPos, Block block)
        {
            FillBlocks(startPos.X, startPos.Y, startPos.Z, endPos.X, endPos.Y, endPos.Z, block);
        }

        [System.Obsolete("Not ready to be used!")]
        public void FillBlocks(int xStart, int yStart, int zStart, int xEnd, int yEnd, int zEnd, Block block)
        {
            try
            {
                m_ChunksToUpdate.Clear();
                // Makes sure the blocks can be filled no matter the coordinates.
                FixValues(ref xStart, ref xEnd);
                FixValues(ref yStart, ref yEnd);
                FixValues(ref zStart, ref zEnd);

                for (int x = xStart; x <= xEnd; x++)
                {
                    for (int y = yStart; y <= yEnd; y++)
                    {
                        for (int z = zStart; z <= zEnd; z++)
                        {
                            BlockPos pos = new BlockPos(x, y, z);
                            Chunk chunk = GetChunk(pos);
                            SetBlock(pos, block, false);

                            if (!m_ChunksToUpdate.ContainsKey(pos) && chunk != null)
                                m_ChunksToUpdate.Add(pos, chunk);
                        }
                    }
                }

                for (int i = 0; i < m_ChunksToUpdate.Count; i++)
                {
                    m_ChunksToUpdate.ElementAt(i).Value.UpdateChunk();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void FixValues(ref int start, ref int end)
        {
            if (start > end)
            {
                int temp = start;
                start = end;
                end = temp;
            }
        }

        /// <summary>
        /// Updates any chunks neighboring a block position
        /// </summary>
        /// <param name="pos">position of change</param>
        public void UpdateAdjacentChunks(BlockPos pos)
        {
            BlockPos localPos = pos - pos.ContainingChunkCoordinates();
            //Checks to see if the block position is on the border of the chunk 
            //and if so update the chunk it's touching
            UpdateIfEqual(localPos.X, 0, pos.Add(-1, 0, 0));
            UpdateIfEqual(localPos.X, Chunk.CHUNK_SIZE - 1, pos.Add(1, 0, 0));
            UpdateIfEqual(localPos.Y, 0, pos.Add(0, -1, 0));
            UpdateIfEqual(localPos.Y, Chunk.CHUNK_SIZE - 1, pos.Add(0, 1, 0));
            UpdateIfEqual(localPos.Z, 0, pos.Add(0, 0, -1));
            UpdateIfEqual(localPos.Z, Chunk.CHUNK_SIZE - 1, pos.Add(0, 0, 1));
        }

        private void UpdateIfEqual(int value1, int value2, BlockPos pos)
        {
            if (value1 == value2)
            {
                Chunk chunk = GetChunk(pos);
                if (chunk != null)
                    chunk.UpdateChunk();
            }
        }

        public void SetSizeX(float value)
        {
            WorldSizeX = Mathf.RoundToInt(value);
        }

        public void SetSizeY(float value)
        {
            WorldSizeY = Mathf.RoundToInt(value);
        }

        public void SetSizeZ(float value)
        {
            WorldSizeZ = Mathf.RoundToInt(value);
        }
    }
}
