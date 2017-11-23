using UnityEngine;

namespace Hertzole.HertzVox
{
    public class TerrainGen : MonoBehaviour
    {
        private World m_World;
        public World World { get { return m_World; } set { m_World = value; } }

        public virtual void OnGenerateChunkColumn(BlockPos pos) { }
    }
}
