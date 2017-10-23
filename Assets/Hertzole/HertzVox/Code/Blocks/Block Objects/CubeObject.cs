using UnityEngine;

namespace Hertzole.HertzVox.Blocks
{
    [CreateAssetMenu(fileName = "New Cube Block", menuName = "HertzVox/Cube Block", order = 2)]
    public class CubeObject : BlockObject
    {
        [SerializeField]
        private bool m_IsSolid = true;
        public bool IsSolid { get { return m_IsSolid; } set { m_IsSolid = value; } }
        [SerializeField]
        private bool m_SolidTowardsSameType = true;
        public bool SolidTowardsSameType { get { return m_SolidTowardsSameType; } set { m_SolidTowardsSameType = value; } }

        public override BlockController Controller()
        {
            BlockCube controller = new BlockCube
            {
                BlockName = BlockName,
                IsSolid = IsSolid,
                SolidTowardsSameType = SolidTowardsSameType
            };

            TextureCollection[] textureCoordinates = new TextureCollection[6];

            if (!IsConnectedTextures)
            {
                for (int i = 0; i < 6; i++)
                {
                    try
                    {
                        textureCoordinates[i] = Block.Index.textureIndex.GetTextureCollection(Textures.GetTextureFromIndex(i).name);
                    }
                    catch
                    {
                        if (Application.isPlaying)
                            Debug.LogError("Couldn't find texture for " + Textures.GetTextureFromIndex(i));
                    }
                }
            }
            else
            {
                for (int i = 0; i < ConnectedTextures.Length; i++)
                {
                    try
                    {
                        textureCoordinates[i] = Block.Index.textureIndex.GetTextureCollection(ConnectedTextures[i].name);
                    }
                    catch
                    {
                        if (Application.isPlaying)
                            Debug.LogError("¯\\_(ツ)_/¯");
                    }
                }
            }

            controller.Textures = textureCoordinates;

            return controller;
        }
    }
}
