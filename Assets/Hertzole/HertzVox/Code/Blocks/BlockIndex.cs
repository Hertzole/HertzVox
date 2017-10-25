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

        public List<BlockController> controllers = new List<BlockController>();
        public List<BlockOverride> blockOverrides = new List<BlockOverride>();
        public Dictionary<string, int> names = new Dictionary<string, int>();

        public TextureIndex textureIndex;

        public int AddBlockType(BlockController controller)
        {
            int index = controllers.Count;

            if (index == ushort.MaxValue)
            {
                Debug.LogError("Too many block types!");
                return -1;
            }

            if (names.ContainsKey(controller.Name()))
            {
                Debug.LogError("Two blocks with the same name " + controller.Name() + " are defined!");
                return -1;
            }

            controllers.Add(controller);
            BlockOverride blockOverride = GetBlockOverride(controller.Name());
            if (blockOverride != null)
                blockOverride.m_Controller = controller;

            blockOverrides.Add(blockOverride);

            names.Add(controller.Name().ToLower().Replace(" ", ""), index);
            return index;
        }

        public void GetMissingDefinitions(BlockCollection blockCollection)
        {
            textureIndex = new TextureIndex();
            textureIndex.Initialize(blockCollection);
            textureIndex.Load();

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
