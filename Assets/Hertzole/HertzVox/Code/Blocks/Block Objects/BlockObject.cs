using UnityEngine;

namespace Hertzole.HertzVox.Blocks
{
    public class BlockObject : ScriptableObject
    {
        [SerializeField]
        private string m_BlockName = "New";
        public string BlockName { get { return m_BlockName; } set { m_BlockName = value; } }
        [SerializeField]
        private BlockTextures m_Textures;
        public BlockTextures Textures { get { return m_Textures; } set { m_Textures = value; } }
        [SerializeField]
        private Texture2D[] m_ConnectedTextures;
        public Texture2D[] ConnectedTextures { get { return m_ConnectedTextures; } set { m_ConnectedTextures = value; } }
        [SerializeField]
        private bool m_IsConnectedTextures = false;
        public bool IsConnectedTextures { get { return m_IsConnectedTextures; } set { m_IsConnectedTextures = value; } }

        public virtual BlockController Controller()
        {
            return new BlockAir();
        }

        public virtual void AddToBlocks()
        {
            Block.Index.AddBlockType(Controller());
        }
    }
}
