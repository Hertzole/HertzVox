using System;
using UnityEngine;

namespace Hertzole.HertzVox
{
    [Serializable]
    public struct BlockPos
    {
        private int m_X, m_Y, m_Z;
        public int X { get { return m_X; } set { m_X = value; } }
        public int Y { get { return m_Y; } set { m_Y = value; } }
        public int Z { get { return m_Z; } set { m_Z = value; } }

        public BlockPos(int x, int y, int z)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        //Overriding GetHashCode and Equals gives us a faster way to
        //compare two positions and we have to do that a lot
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 47;

                hash = hash * 227 + X.GetHashCode();
                hash = hash * 227 + Y.GetHashCode();
                hash = hash * 227 + Z.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (GetHashCode() == obj.GetHashCode())
                return true;

            return false;
        }

        //returns the position of the chunk containing this block
        public BlockPos ContainingChunkCoordinates()
        {
            int x = Mathf.FloorToInt(this.X / (float)Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE;
            int y = Mathf.FloorToInt(this.Y / (float)Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE;
            int z = Mathf.FloorToInt(this.Z / (float)Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE;

            return new BlockPos(x, y, z);
        }

        public BlockPos Add(int x, int y, int z)
        {
            return new BlockPos(this.X + x, this.Y + y, this.Z + z);
        }

        public BlockPos Add(BlockPos pos)
        {
            return new BlockPos(this.X + pos.X, this.Y + pos.Y, this.Z + pos.Z);
        }

        public BlockPos Subtract(BlockPos pos)
        {
            return new BlockPos(this.X - pos.X, this.Y - pos.Y, this.Z - pos.Z);
        }

        //BlockPos and Vector3 can be substituted for one another
        public static implicit operator BlockPos(Vector3 v)
        {
            BlockPos blockPos = new BlockPos(
                Mathf.RoundToInt(v.x / HertzVoxConfig.BlockSize),
                Mathf.RoundToInt(v.y / HertzVoxConfig.BlockSize),
                Mathf.RoundToInt(v.z / HertzVoxConfig.BlockSize)
                );

            return blockPos;
        }

        public static implicit operator Vector3(BlockPos pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z) * HertzVoxConfig.BlockSize;
        }

        public static implicit operator BlockPos(Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    return new BlockPos(0, 1, 0);
                case Direction.Down:
                    return new BlockPos(0, -1, 0);
                case Direction.North:
                    return new BlockPos(0, 0, 1);
                case Direction.East:
                    return new BlockPos(1, 0, 0);
                case Direction.South:
                    return new BlockPos(0, 0, -1);
                case Direction.West:
                    return new BlockPos(-1, 0, 0);
                default:
                    return new BlockPos();
            }
        }

        //These operators let you add and subtract BlockPos from each other
        //or check equality with == and !=
        public static BlockPos operator -(BlockPos pos1, BlockPos pos2)
        {
            return pos1.Subtract(pos2);
        }

        public static BlockPos operator +(BlockPos pos1, BlockPos pos2)
        {
            return pos1.Add(pos2);
        }

        public static bool operator ==(BlockPos pos1, BlockPos pos2)
        {
            return Equals(pos1, pos2);
        }

        public static bool operator !=(BlockPos pos1, BlockPos pos2)
        {
            return !Equals(pos1, pos2);
        }

        //You can safely use BlockPos as part of a string like this:
        //"block at " + BlockPos + " is broken."
        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}
