using SimplexNoise;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public class TextureCollection
    {
        private string m_TextureName;
        public string TextureName { get { return m_TextureName; } }
        private bool m_UsesConnectedTextures = false;
        public bool UsesConnectedTextures { get { return m_UsesConnectedTextures; } }

        private Rect[] m_ConnectedTextures = new Rect[48];
        private List<Rect> m_Textures = new List<Rect>();
        private Noise m_NoiseGen;

        public TextureCollection(string name)
        {
            m_TextureName = name;
            m_NoiseGen = new Noise();
        }

        public void AddTexture(Rect texture, int connectedTextureType)
        {
            if (connectedTextureType != -1)
            {
                m_UsesConnectedTextures = true;
                m_ConnectedTextures[connectedTextureType] = texture;
            }
            else
                m_Textures.Add(texture);
        }

        public Rect GetTexture(Chunk chunk, BlockPos pos, Direction direction)
        {
            if (UsesConnectedTextures)
            {
                string blockName = chunk.GetBlock(pos).Controller.Name();

                bool wn = ConnectedTextures.IsSame(chunk, pos, -1, 1, direction, blockName);
                bool n = ConnectedTextures.IsSame(chunk, pos, 0, 1, direction, blockName);
                bool ne = ConnectedTextures.IsSame(chunk, pos, 1, 1, direction, blockName);
                bool w = ConnectedTextures.IsSame(chunk, pos, -1, 0, direction, blockName);
                bool e = ConnectedTextures.IsSame(chunk, pos, 1, 0, direction, blockName);
                bool es = ConnectedTextures.IsSame(chunk, pos, 1, -1, direction, blockName);
                bool s = ConnectedTextures.IsSame(chunk, pos, 0, -1, direction, blockName);
                bool sw = ConnectedTextures.IsSame(chunk, pos, -1, -1, direction, blockName);

                return m_ConnectedTextures[ConnectedTextures.GetTexture(n, e, s, w, wn, ne, es, sw)];
            }

            if (m_Textures.Count == 1)
                return m_Textures[0];

            if (m_Textures.Count > 1)
            {
                float randomNumber = m_NoiseGen.Generate(pos.x, pos.y, pos.z);
                randomNumber += 1;
                randomNumber /= 2;
                randomNumber *= m_Textures.Count;

                return m_Textures[(int)randomNumber];
            }

            Debug.LogError("There were no textures for " + TextureName);
            return new Rect();
        }
    }
}
