using Hertzole.HertzVox.Blocks;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public class TextureIndex
    {
        private Dictionary<string, TextureCollection> m_Textures = new Dictionary<string, TextureCollection>();
        public Dictionary<string, TextureCollection> Textures { get { return m_Textures; } set { m_Textures = value; } }

        private Texture2D m_Atlas;
        public Texture2D Atlas { get { return m_Atlas; } }

        private bool m_HasInited = false;

        private List<Texture2D> m_NormalTextures = new List<Texture2D>();
        private List<Texture2D> m_ConnectedTextures = new List<Texture2D>();

        public void Initialize(BlockCollection collection)
        {
            for (int i = 0; i < collection.Blocks.Length; i++)
            {
                if (collection.Blocks[i].IsConnectedTextures)
                    m_ConnectedTextures.AddRange(collection.Blocks[i].ConnectedTextures);
                else
                    m_NormalTextures.AddRange(collection.Blocks[i].Textures.GetTextures());
            }

            m_HasInited = true;
        }

        public void Load(bool loadEmpty = false)
        {
            if (!m_HasInited)
            {
                Debug.LogError("TextureIndex needs to be Initialized first!");
                return;
            }

            if (loadEmpty)
                return;

            List<Texture2D> atlasTextures = new List<Texture2D>();

            atlasTextures.AddRange(m_NormalTextures);

            if (m_ConnectedTextures.Count != 0)
                atlasTextures.AddRange(ConnectedTextures.GenerateConnectedTextures(m_ConnectedTextures));

            m_Atlas = new Texture2D(8192, 8192)
            {
                filterMode = FilterMode.Point
            };

            Rect[] rects = m_Atlas.PackTextures(atlasTextures.ToArray(), 0, 8192, false);

            for (int i = 0; i < atlasTextures.Count; i++)
            {


                if (!atlasTextures[i])
                    continue;

                string[] fileName = atlasTextures[i].name.ToString().Split('-');
                Rect texture = rects[i];

                string textureName = fileName[0];
                int connectedTextureType = -1;

                for (int n = 0; n < fileName.Length; n++)
                {
                    switch (fileName[n][0])
                    {
                        case 'c':
                            int.TryParse(fileName[n].Substring(1), out connectedTextureType);
                            break;
                        default:
                            break;
                    }
                }

                TextureCollection collection;
                if (!Textures.TryGetValue(textureName, out collection))
                {
                    collection = new TextureCollection(textureName);
                    Textures.Add(textureName, collection);
                }

                collection.AddTexture(texture, connectedTextureType);
            }
        }

        public TextureCollection GetTextureCollection(string textureName)
        {
            string[] fileName = textureName.Split('-');
            textureName = fileName[0];

            TextureCollection collection;
            Textures.TryGetValue(textureName, out collection);
            return collection;
        }
    }
}
