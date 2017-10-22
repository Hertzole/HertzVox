using Hertzole.HertzVox.Blocks.Builders;

namespace Hertzole.HertzVox.Blocks
{
    public class BlockSlope : BlockSolid
    {
        private string m_BlockName;
        public string BlockName { get { return m_BlockName; } set { m_BlockName = value; } }
        private TextureCollection[] m_Textures;
        public TextureCollection[] Textures { get { return m_Textures; } set { m_Textures = value; } }

        public override void BuildFace(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, Block block)
        {
            //Debug.LogWarning(chunk.gameObject + " BuildFace");
            SlopeBuilder.BuildRenderer(chunk, pos, meshData, direction);
            SlopeBuilder.BuildTexture(chunk, pos, meshData, direction, Textures);
            SlopeBuilder.BuildColors(chunk, pos, meshData, direction);
            //TODO: Implement greedy mesh builder.
            SlopeBuilder.BuildCollider(chunk, pos, meshData, direction);
        }

        public override string Name()
        {
            return BlockName;
        }

        public override bool IsBlockSolid(Direction direction)
        {
            return IsSolid;
        }
    }
}
