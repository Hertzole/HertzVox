using Hertzole.HertzVox.Blocks;
using Hertzole.HertzVox.Experimental;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Hertzole.HertzVox
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        private Block[,,] m_Blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        public Block[,,] Blocks { get { return m_Blocks; } }

        private List<BlockAndTimer> m_ScheduledUpdates = new List<BlockAndTimer>();

        public enum Flag { Busy, MeshReady, Loaded, TerrainGenerated, MarkedForDeletion, QueuedForUpdate, ChunkModified, UpdateSoon }
        private Dictionary<Flag, bool> m_Flags = new Dictionary<Flag, bool>();
        public Dictionary<Flag, bool> Flags { get { return m_Flags; } set { m_Flags = value; } }

        private MeshFilter m_Filter;
        private MeshFilter Filter { get { if (!m_Filter) m_Filter = GetComponent<MeshFilter>(); return m_Filter; } }
        private MeshCollider m_Collider;
        public MeshCollider Collider { get { if (!m_Collider) m_Collider = GetComponent<MeshCollider>(); return m_Collider; } }

        private World m_World;
        public World World { get { return m_World; } set { m_World = value; } }
        private BlockPos m_Position;
        public BlockPos Position { get { return m_Position; } set { m_Position = value; } }
        private MeshRenderer m_Renderer;
        public MeshRenderer Renderer { get { if (!m_Renderer) m_Renderer = GetComponent<MeshRenderer>(); return m_Renderer; } }

        private float m_RandomUpdateTime = 0;

        private MeshData m_MeshData = new MeshData();

        public const int CHUNK_SIZE = 16;

        private void Start()
        {
            Renderer.material.mainTexture = Block.Index.TextureIndex.Atlas;
        }

        public bool GetFlag(Flag flag)
        {
            if (!Flags.ContainsKey(flag))
                return false;

            return Flags[flag];
        }

        public void SetFlag(Flag flag, bool value)
        {
            if (Flags.ContainsKey(flag))
                Flags.Remove(flag);

            Flags.Add(flag, value);
        }

        private void Update()
        {
            if (GetFlag(Flag.MarkedForDeletion) && !GetFlag(Flag.Busy))
                ReturnChunkToPool();

            if (GetFlag(Flag.MeshReady))
            {
                SetFlag(Flag.MeshReady, false);
                RenderMesh();
                m_MeshData = new MeshData();
                SetFlag(Flag.Busy, false);
            }
        }

        private void FixedUpdate()
        {
            m_RandomUpdateTime += Time.fixedDeltaTime;

            if (m_RandomUpdateTime >= 0.1f)
            {
                if (GetFlag(Flag.UpdateSoon))
                {
                    UpdateChunk();
                    SetFlag(Flag.UpdateSoon, false);
                }

                m_RandomUpdateTime = 0;

                BlockPos randomPos = new BlockPos
                {
                    x = World.Random.Next(0, 16),
                    y = World.Random.Next(0, 16),
                    z = World.Random.Next(0, 16)
                };

                GetBlock(randomPos).Controller.RandomUpdate(this, randomPos, GetBlock(randomPos));

                ProcessScheduledUpdates();
            }
        }

        private void ProcessScheduledUpdates()
        {
            for (int i = 0; i < m_ScheduledUpdates.Count; i++)
            {
                m_ScheduledUpdates[i] = new BlockAndTimer(m_ScheduledUpdates[i].Pos, m_ScheduledUpdates[i].Time - 0.1f);
                if (m_ScheduledUpdates[i].Time <= 0)
                {
                    Block block = GetBlock(m_ScheduledUpdates[i].Pos);
                    block.Controller.ScheduledUpdate(this, m_ScheduledUpdates[i].Pos, block);
                    m_ScheduledUpdates.RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddScheduledUpdate(BlockPos pos, float time)
        {
            m_ScheduledUpdates.Add(new BlockAndTimer(pos, time));
        }

        /// <summary>
        /// Updates the chunk either now or as soon as the chunk is no longer busy
        /// </summary>
        public void UpdateChunk()
        {
            if (HertzVoxConfig.UseMultiThreading)
            {
                Thread thread = new Thread(() =>
                {
                    // If there's already an update queued let that one run instead
                    if (!GetFlag(Flag.QueuedForUpdate))
                    {
                        // If the chunk is busy wait for it to be ready, but
                        // set a flag saying an update is waiting so that later
                        // updates don't sit around as well, one is enough
                        if (GetFlag(Flag.Busy))
                        {
                            SetFlag(Flag.QueuedForUpdate, true);
                            while (GetFlag(Flag.Busy))
                                Thread.Sleep(0);

                            SetFlag(Flag.QueuedForUpdate, false);
                        }

                        SetFlag(Flag.Busy, true);
                        BuildMeshData();
                        SetFlag(Flag.MeshReady, true);
                    }
                });
                thread.Start();
            }
            else
            {
                SetFlag(Flag.Busy, true);
                BuildMeshData();
                SetFlag(Flag.MeshReady, true);
            }
        }

        /// <summary>
        /// Gets and returns a block from a local position within the chunk 
        /// or fetches it from the world
        /// </summary>
        /// <param name="blockPos">A local block position</param>
        /// <returns>The block at the position</returns>
        public Block GetBlock(BlockPos blockPos)
        {
            Block returnBlock;

            if (InRange(blockPos))
                returnBlock = Blocks[blockPos.x, blockPos.y, blockPos.z];
            else
                returnBlock = World.GetBlock(blockPos + Position);

            return returnBlock;
        }

        /// <summary>
        /// Returns true if the block local block position is contained in the chunk boundaries
        /// </summary>
        /// <param name="localPos">A local block position</param>
        /// <returns>true or false depending on if the position is in range</returns>
        public static bool InRange(BlockPos localPos)
        {
            if (!InRange(localPos.x))
                return false;
            if (!InRange(localPos.y))
                return false;
            if (!InRange(localPos.z))
                return false;

            return true;
        }

        public static bool InRange(int index)
        {
            if (index < 0 || index >= CHUNK_SIZE)
                return false;

            return true;
        }

        public void SetBlock(int x, int y, int z, Block block, bool updateChunk = true)
        {
            SetBlock(new BlockPos(x, y, z), block, updateChunk);
        }

        /// <summary>
        /// Sets the block at the given local position
        /// </summary>
        /// <param name="blockPos">Local position</param>
        /// <param name="block">Block to place at the given location</param>
        /// <param name="updateChunk">Optional parameter, set to false to keep the chunk unupdated despite the change</param>
        public void SetBlock(BlockPos blockPos, Block block, bool updateChunk = true)
        {
            if (InRange(blockPos))
            {
                // Only call create and destroy if this is a different block type, otherwise it's just updating the properties of an existing block
                if (Blocks[blockPos.x, blockPos.y, blockPos.z].type != block.type)
                {
                    Blocks[blockPos.x, blockPos.y, blockPos.z].Controller.OnDestroy(this, blockPos + Position, Blocks[blockPos.x, blockPos.y, blockPos.z]);
                    block = block.Controller.OnCreate(this, blockPos, block);
                }

                Blocks[blockPos.x, blockPos.y, blockPos.z] = block;

                if (block.Modified)
                    SetFlag(Flag.ChunkModified, true);

                if (updateChunk)
                    UpdateChunk();
            }
            else
            {
                World.SetBlock(blockPos + Position, block, updateChunk);
            }
        }

        /// <summary>
        /// Updates the chunk based on its contents
        /// </summary>
        private void BuildMeshData()
        {
            try
            {
                for (int x = 0; x < CHUNK_SIZE; x++)
                {
                    for (int y = 0; y < CHUNK_SIZE; y++)
                    {
                        for (int z = 0; z < CHUNK_SIZE; z++)
                        {
                            m_Blocks[x, y, z].Controller.BuildBlock(this, new BlockPos(x, y, z), m_MeshData, m_Blocks[x, y, z]);
                        }
                    }
                }

                if (HertzVoxConfig.UseGreedyCollider)
                    MergedFaceMeshBuilder.ReduceMesh(this, m_MeshData);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        /// <summary>
        /// Sends the calculated mesh information
        /// to the mesh and collision components
        /// </summary>
        private void RenderMesh()
        {
            Filter.mesh.Clear();
            Filter.mesh.vertices = m_MeshData.Vertices.ToArray();
            Filter.mesh.triangles = m_MeshData.Triangles.ToArray();

            Filter.mesh.colors = m_MeshData.Colors.ToArray();

            Filter.mesh.uv = m_MeshData.UV.ToArray();
            Filter.mesh.RecalculateNormals();

            Collider.sharedMesh = null;
            Mesh colliderMesh = new Mesh
            {
                vertices = m_MeshData.ColliderVertices.ToArray(),
                triangles = m_MeshData.ColliderTriangles.ToArray()
            };
            colliderMesh.RecalculateNormals();

            Collider.sharedMesh = colliderMesh;
        }

        public void MarkForDeletion()
        {
            SetFlag(Flag.MarkedForDeletion, true);
        }

        public bool IsMarkedForDeletion()
        {
            return GetFlag(Flag.MarkedForDeletion);
        }

        private void ReturnChunkToPool()
        {
            Flags.Clear();

            if (Filter.mesh)
                Filter.mesh.Clear();

            if (Collider.sharedMesh)
                Collider.sharedMesh.Clear();

            m_Blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
            m_MeshData = new MeshData();

            World.AddToChunkPool(this);
        }

        public bool IsEmpty()
        {
            bool empty = true;

            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        if (m_Blocks[x, y, z] != Block.Air)
                            empty = false;
                    }
                }
            }

            return empty;
        }
    }
}
