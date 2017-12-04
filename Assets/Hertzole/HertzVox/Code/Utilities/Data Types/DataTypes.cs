using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzVox
{
    [Flags]
    public enum Direction : byte
    {
        North = 0x04,
        East = 0x10,
        South = 0x08,
        West = 0x20,
        Up = 0x01,
        Down = 0x02
    };

    public struct Tile
    {
        private int m_X;
        public int X { get { return m_X; } set { m_X = value; } }
        private int m_Y;
        public int Y { get { return m_Y; } set { m_Y = value; } }

        public Tile(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }
    }

    public struct BlockAndTimer
    {
        private BlockPos m_Pos;
        public BlockPos Pos { get { return m_Pos; } set { m_Pos = value; } }
        private float m_Time;
        public float Time { get { return m_Time; } set { m_Time = value; } }

        public BlockAndTimer(BlockPos pos, float time)
        {
            m_Pos = pos;
            m_Time = time;
        }
    }

    [System.Serializable]
    public class BlockTextures
    {
        [SerializeField]
        private Texture2D m_Top;
        public Texture2D Top { get { return m_Top; } set { m_Top = value; } }
        [SerializeField]
        private Texture2D m_Bottom;
        public Texture2D Bottom { get { return m_Bottom; } set { m_Bottom = value; } }
        [SerializeField]
        private Texture2D m_Front;
        public Texture2D Front { get { return m_Front; } set { m_Front = value; } }
        [SerializeField]
        private Texture2D m_Back;
        public Texture2D Back { get { return m_Back; } set { m_Back = value; } }
        [SerializeField]
        private Texture2D m_Right;
        public Texture2D Right { get { return m_Right; } set { m_Right = value; } }
        [SerializeField]
        private Texture2D m_Left;
        public Texture2D Left { get { return m_Left; } set { m_Left = value; } }

        public Texture2D GetTextureFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return m_Top;
                case 1:
                    return m_Bottom;
                case 2:
                    return m_Front;
                case 3:
                    return m_Right;
                case 4:
                    return m_Back;
                case 5:
                    return m_Left;
                default:
                    return null;
            }
        }

        public Texture2D[] GetTextures()
        {
            List<Texture2D> list = new List<Texture2D> { Top, Bottom, Front, Back, Right, Left };
            return list.ToArray();
        }
    }
}
