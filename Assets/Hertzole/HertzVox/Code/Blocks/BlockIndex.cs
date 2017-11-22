using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzVox.Blocks
{
    public class BlockIndex
    {
        public BlockIndex()
        {
            AddBlockType(new BlockAir());
            AddBlockType(new BlockSolid());
        }

        private List<BlockController> m_Controllers = new List<BlockController>();
        public List<BlockController> Controllers { get { return m_Controllers; } set { m_Controllers = value; } }
        private List<BlockOverride> m_BlockOverrides = new List<BlockOverride>();
        public List<BlockOverride> BlockOverrides { get { return m_BlockOverrides; } set { m_BlockOverrides = value; } }
        private Dictionary<string, int> m_Names = new Dictionary<string, int>();
        public Dictionary<string, int> Names { get { return m_Names; } set { m_Names = value; } }

        private TextureIndex m_TextureIndex;
        public TextureIndex TextureIndex { get { return m_TextureIndex; } set { m_TextureIndex = value; } }

        public int AddBlockType(BlockController controller)
        {
            int index = Controllers.Count;

            if (index == ushort.MaxValue)
            {
                Debug.LogError("Too many block types!");
                return -1;
            }

            if (Names.ContainsKey(controller.Name()))
            {
                Debug.LogError("Two blocks with the same name " + controller.Name() + " are defined!");
                return -1;
            }

            Controllers.Add(controller);
            BlockOverride blockOverride = GetBlockOverride(controller.Name());
            if (blockOverride != null)
                blockOverride.m_Controller = controller;

            BlockOverrides.Add(blockOverride);

            Names.Add(controller.Name().ToLower().Replace(" ", ""), index);
            return index;
        }

        public void GetMissingDefinitions(BlockCollection blockCollection)
        {
            TextureIndex = new TextureIndex();
            TextureIndex.Initialize(blockCollection);
            TextureIndex.Load();

            foreach (var def in blockCollection.Blocks)
            {
                def.AddToBlocks();
            }
        }

        BlockOverride GetBlockOverride(string blockName)
        {
            var type = Type.GetType(blockName + "Override" + ", " + typeof(BlockOverride).Assembly, false);
            if (type == null)
                return null;

            return (BlockOverride)Activator.CreateInstance(type);
        }
    }
}
