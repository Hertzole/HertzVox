namespace Hertzole.HertzVox.Blocks
{
    public class BlockAir : BlockController
    {
        public BlockAir() : base() { }

        public override bool IsBlockSolid(Direction direction)
        {
            return false;
        }

        public override string Name()
        {
            return "air";
        }

        public override bool IsTransparent()
        {
            return true;
        }
    }
}
