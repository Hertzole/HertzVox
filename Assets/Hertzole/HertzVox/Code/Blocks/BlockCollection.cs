using UnityEngine;

namespace Hertzole.HertzVox.Blocks
{
    [CreateAssetMenu(fileName = "New Block Collection", menuName = "HertzVox/Block Collection", order = 1)]
    public class BlockCollection : ScriptableObject
    {
        [SerializeField]
        private BlockObject[] m_Blocks;
        public BlockObject[] Blocks { get { return m_Blocks; } set { m_Blocks = value; } }

        //[SerializeField]
        //private Texture2D blocksTexture;
        //public Texture2D BlocksTexture { get { return blocksTexture; } set { blocksTexture = value; } }
        //[SerializeField]
        //private int tileSize = 32;
        //public int TileSize { get { return tileSize; } set { tileSize = value; } }
        //[Space]
        //[SerializeField]
        //private List<Block> blocks = new List<Block>();
        ////public List<Block> Blocks { get { return blocks; } set { blocks = value; } }
        //public int BlocksCount { get { return blocks.Count; } }

        //public Block GetBlock(string id)
        //{
        //    Block blockToReturn = null;

        //    for (int i = 0; i < blocks.Count; i++)
        //    {
        //        int index = i;
        //        if (blocks[i].ID == id)
        //        {
        //            blockToReturn = blocks[i];
        //            blockToReturn.IndexID = index;
        //            //Debug.Log("Returning " + blockToReturn.Name + " with index ID " + blockToReturn.IndexID);
        //        }
        //    }

        //    if (blockToReturn == null)
        //        Debug.LogError("No block with the ID '" + id + "' was found in " + name + "!");

        //    return blockToReturn;
        //}
    }
}
