using Hertzole.HertzVox.Blocks;
using System.Collections;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public class BlockController
    {
        public BlockController() { }

        public virtual void AddBlockData(Chunk chunk, BlockPos pos, MeshData meshData, Block block) { }

        public virtual void BuildBlock(Chunk chunk, BlockPos pos, MeshData meshData, Block block)
        {
            PreRender(chunk, pos, block);
            AddBlockData(chunk, pos, meshData, block);
            PostRender(chunk, pos, block);
        }

        public virtual string Name() { return "BlockController"; }

        public virtual bool IsBlockSolid(Direction direction) { return true; }

        public virtual bool IsTransparent() { return false; }

        public virtual byte LightEmitted() { return 0; }

        public virtual bool CanBeWalkedOn(Block block) { return false; }

        public virtual bool CanBeWalkedThrough(Block block) { return true; }

        private Hashtable m_Flags = new Hashtable();
        public Hashtable Flags { get { return m_Flags; } set { m_Flags = value; } }

        T GetFlag<T>(object key) where T : new()
        {
            if (!Flags.ContainsKey(key))
                return new T();
            return (T)Flags[key];
        }

        public void SetFlag(object key, object value)
        {
            if (Flags.ContainsKey(key))
                Flags.Remove(key);
            Flags.Add(key, value);
        }

        public virtual T GetFlagOrOverride<T>(Object key, Chunk chunk, BlockPos pos, Block block) where T : new()
        {
            if (BlockOverride.GetBlockOverride(block.Type) != null)
            {
                System.Object overridenReturn = BlockOverride.GetBlockOverride(block.Type).GetFlagIntercept(key, chunk, pos, block);
                if (overridenReturn != null)
                    return (T)overridenReturn;
            }

            return GetFlag<T>(key);
        }

        public virtual Block OnCreate(Chunk chunk, BlockPos pos, Block block)
        {
            if (BlockOverride.GetBlockOverride(block.Type) == null)
                return block;

            return BlockOverride.GetBlockOverride(block.Type).OnCreate(chunk, pos, block);
        }

        public virtual void PreRender(Chunk chunk, BlockPos pos, Block block)
        {
            if (BlockOverride.GetBlockOverride(block.Type) != null)
                BlockOverride.GetBlockOverride(block.Type).PreRender(chunk, pos, block);
        }

        public virtual void PostRender(Chunk chunk, BlockPos pos, Block block)
        {
            if (BlockOverride.GetBlockOverride(block.Type) != null)
                BlockOverride.GetBlockOverride(block.Type).PostRender(chunk, pos, block);
        }

        public virtual void OnDestroy(Chunk chunk, BlockPos pos, Block block)
        {
            if (BlockOverride.GetBlockOverride(block.Type) != null)
                BlockOverride.GetBlockOverride(block.Type).OnDestroy(chunk, pos, block);
        }

        public virtual void RandomUpdate(Chunk chunk, BlockPos pos, Block block)
        {
            if (BlockOverride.GetBlockOverride(block.Type) != null)
                BlockOverride.GetBlockOverride(block.Type).RandomUpdate(chunk, pos, block);
        }

        public virtual void ScheduledUpdate(Chunk chunk, BlockPos pos, Block block)
        {
            if (BlockOverride.GetBlockOverride(block.Type) != null)
                BlockOverride.GetBlockOverride(block.Type).ScheduledUpdate(chunk, pos, block);
        }
    }
}
