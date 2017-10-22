using UnityEngine;

namespace Hertzole.HertzVox.Commands
{
    public class SetBlockCommand : Command
    {
        public SetBlockCommand() : base("setblock") { }

        public override void Execute(string[] arguments)
        {
            int x, y, z;
            if (!int.TryParse(arguments[0], out x))
            {
                Debug.LogWarning("Can't convert X!");
                return;
            }

            if (!int.TryParse(arguments[1], out y))
            {
                Debug.LogWarning("Can't convert Y!");
                return;
            }

            if (!int.TryParse(arguments[2], out z))
            {
                Debug.LogWarning("Can't convert Z!");
                return;
            }

            string block = arguments[3];
            //if (!int.TryParse(arguments[3], out block))
            //{
            //    Debug.LogWarning("Can't convert block!");
            //    return;
            //}

            //if (block < 0 || block >= blocks.Length)
            //{
            //    Debug.LogWarning("Block is out of bounds!");
            //    return;
            //}

            //World.Instance.SetBlock(x, y, z, blocks[block]);
            Debug.Log("Set block command");
            World.Instance.SetBlock(new BlockPos(x, y, z), block);
        }
    }
}
