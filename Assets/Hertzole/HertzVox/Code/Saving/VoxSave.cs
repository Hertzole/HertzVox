using System;
using System.Collections.Generic;

namespace Hertzole.HertzVox.Saving
{
    [Serializable]
    public class VoxSave
    {
        public List<VoxSaveChunk> chunks = new List<VoxSaveChunk>();
    }
}
