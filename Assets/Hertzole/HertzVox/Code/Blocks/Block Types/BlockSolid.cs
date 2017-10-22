namespace Hertzole.HertzVox.Blocks
{
    public class BlockSolid : BlockController
    {
        private bool m_IsSolid = true;
        public bool IsSolid { get { return m_IsSolid; } set { m_IsSolid = value; } }
        private bool m_SolidTowardsSameType = true;
        public bool SolidTowardsSameType { get { return m_SolidTowardsSameType; } set { m_SolidTowardsSameType = value; } }

        public BlockSolid() : base() { }

        public override void AddBlockData(Chunk chunk, BlockPos pos, MeshData meshData, Block block)
        {
            //Debug.Log("AddBlockData");
            //Debug.Log(chunk.gameObject.name + " AddBlockData");

            if (!chunk.GetBlock(pos.Add(0, 1, 0)).Controller.IsBlockSolid(Direction.Down) && (IsSolid || !SolidTowardsSameType || chunk.GetBlock(pos.Add(0, 1, 0)) != block))
                BuildFace(chunk, pos, meshData, Direction.Up, block);

            if (!chunk.GetBlock(pos.Add(0, -1, 0)).Controller.IsBlockSolid(Direction.Up) && (IsSolid || !SolidTowardsSameType || chunk.GetBlock(pos.Add(0, -1, 0)) != block))
                BuildFace(chunk, pos, meshData, Direction.Down, block);

            if (!chunk.GetBlock(pos.Add(0, 0, 1)).Controller.IsBlockSolid(Direction.South) && (IsSolid || !SolidTowardsSameType || chunk.GetBlock(pos.Add(0, 0, 1)) != block))
                BuildFace(chunk, pos, meshData, Direction.North, block);

            if (!chunk.GetBlock(pos.Add(0, 0, -1)).Controller.IsBlockSolid(Direction.North) && (IsSolid || !SolidTowardsSameType || chunk.GetBlock(pos.Add(0, 0, -1)) != block))
                BuildFace(chunk, pos, meshData, Direction.South, block);

            if (!chunk.GetBlock(pos.Add(1, 0, 0)).Controller.IsBlockSolid(Direction.West) && (IsSolid || !SolidTowardsSameType || chunk.GetBlock(pos.Add(1, 0, 0)) != block))
                BuildFace(chunk, pos, meshData, Direction.East, block);

            if (!chunk.GetBlock(pos.Add(-1, 0, 0)).Controller.IsBlockSolid(Direction.East) && (IsSolid || !SolidTowardsSameType || chunk.GetBlock(pos.Add(-1, 0, 0)) != block))
                BuildFace(chunk, pos, meshData, Direction.West, block);
        }

        public virtual void BuildFace(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, Block block) { }

        public override string Name() { return "solid"; }

        public override bool IsBlockSolid(Direction direction) { return false; }

        public override bool CanBeWalkedOn(Block block) { return true; }

        public override bool CanBeWalkedThrough(Block block) { return false; }
    }
}
