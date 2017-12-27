using System;
using UnityEngine;

namespace Hertzole.HertzVox.Blocks
{
    [Serializable]
    public struct Block
    {
        public ushort type;
        public byte data1;
        public byte data2;
        public byte data3;
        public byte data4;

        private bool m_Modified;
        public bool Modified { get { return m_Modified; } set { m_Modified = value; } }

        private static BlockIndex index = new BlockIndex();
        public static BlockIndex Index { get { return index; } }

        public BlockController Controller { get { if (type >= Index.Controllers.Count) Debug.LogError("Block " + type + "is out of range."); return Index.Controllers[type]; } }

        // Reserved block types
        public static Block Void { get { return new Block(ushort.MaxValue); } }

        public static Block Air { get { return new Block(0); } }

        public Block(int type)
        {
            this.type = (ushort)type;
            m_Modified = true;
            data1 = 0;
            data2 = 0;
            data3 = 0;
            data4 = 0;
        }

        public Block(int type, byte data1, byte data2, byte data3, byte data4)
        {
            this.type = (ushort)type;
            m_Modified = true;
            this.data1 = data1;
            this.data2 = data2;
            this.data3 = data3;
            this.data4 = data4;
        }

        public static implicit operator BlockController(Block block)
        {
            return Index.Controllers[block.type];
        }

        public override string ToString()
        {
            return Index.Controllers[type].Name();
        }

        public static implicit operator ushort(Block block)
        {
            return block.type;
        }

        public static implicit operator Block(int b)
        {
            return new Block((ushort)b);
        }

        public static implicit operator int(Block block)
        {
            return block.type;
        }

        public static implicit operator Block(ushort b)
        {
            return new Block(b);
        }

        public static implicit operator Block(string s)
        {
            int blockIndex = 0;
            s = s.ToLower().Replace(" ", "");
            if (Index.Names.TryGetValue(s, out blockIndex))
                return blockIndex;

            Debug.LogWarning("Block not found: " + s);
            return 0;
        }
    }
}
