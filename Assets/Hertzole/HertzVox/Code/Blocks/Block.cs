using System;
using UnityEngine;

namespace Hertzole.HertzVox.Blocks
{
    [Serializable]
    public struct Block
    {
        private ushort m_Type;
        public ushort Type { get { return m_Type; } }
        private byte m_Data1;
        public byte Data1 { get { return m_Data1; } set { m_Data1 = value; } }
        private byte m_Data2;
        public byte Data2 { get { return m_Data2; } set { m_Data2 = value; } }
        private byte m_Data3;
        public byte Data3 { get { return m_Data3; } set { m_Data3 = value; } }
        private byte m_Data4;
        public byte Data4 { get { return m_Data4; } set { m_Data4 = value; } }

        private bool m_Modified;
        public bool Modified { get { return m_Modified; } set { m_Modified = value; } }

        private static BlockIndex index = new BlockIndex();
        public static BlockIndex Index { get { return index; } }

        public BlockController Controller { get { if (Type >= Index.Controllers.Count) Debug.LogError("Block " + Type + "is out of range."); return Index.Controllers[Type]; } }

        // Reserved block types
        public static Block Void { get { return new Block(ushort.MaxValue); } }

        public static Block Air { get { return new Block(0); } }

        public Block(int type)
        {
            m_Type = (ushort)type;
            m_Modified = true;
            m_Data1 = 0;
            m_Data2 = 0;
            m_Data3 = 0;
            m_Data4 = 0;
        }

        public static implicit operator BlockController(Block block)
        {
            return Index.Controllers[block.Type];
        }

        public override string ToString()
        {
            return Index.Controllers[Type].Name();
        }

        public static implicit operator ushort(Block block)
        {
            return block.Type;
        }

        public static implicit operator Block(int b)
        {
            return new Block((ushort)b);
        }

        public static implicit operator int(Block block)
        {
            return block.Type;
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
