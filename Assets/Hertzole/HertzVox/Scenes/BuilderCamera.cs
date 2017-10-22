using Hertzole.HertzVox.Blocks;
using Hertzole.HertzVox.Commands;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public class BuilderCamera : MonoBehaviour
    {
        //private Block[] blocks = new Block[] { BlockCollection.Blocks[0], BlockCollection.Blocks[1], BlockCollection.Blocks[2], BlockCollection.Blocks[3] };
        public BlockCollection blockCollection;
        private Command[] commands = new Command[] { new SetBlockCommand() };

        private string selectedBlock = "air";

        private bool enableWireframe = false;
        private bool lookAround = true;
        private bool dragging = false;
        private bool overGUI = false;

        private Camera cam;
        private BlockPos lookingAtBlockPos;

        private BlockPos dragStart;
        private BlockPos dragEnd;

        //private WorldPos dragStart, dragEnd;

        // Use this for initialization
        void Start()
        {
            cam = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                enableWireframe = !enableWireframe;
                cam.clearFlags = enableWireframe ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
            }

            lookAround = Input.GetMouseButton(1);
            Cursor.lockState = lookAround ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lookAround;

            HandleBuilding();
            HandleMouse();
            HandleMovement();
        }

        void HandleBuilding()
        {
            if (overGUI)
                return;

            if (lookAround)
                return;

            if (!overGUI)
            {
                //if (Input.GetMouseButtonDown(0) && !overGUI)
                //{
                //    dragging = true;
                //    dragStart = GetMouseHitPosition().point;
                //}

                //if (Input.GetMouseButton(0) && dragging && !overGUI)
                //{
                //    dragEnd = GetMouseHitPosition().point;
                //}

                //if (Input.GetMouseButtonUp(0) && dragging && !overGUI)
                //{
                //    dragging = false;
                //    dragEnd = GetMouseHitPosition().point;

                //    VoxelTerrain.FillBlock(dragStart, dragEnd, new BlockAir());
                //}
                if (Input.GetMouseButtonDown(0) && !overGUI)
                {
                    World.Instance.SetBlock(GetMouseHitPosition().point, selectedBlock, true);
                }
            }

            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            //{
            //    lookingAtBlockPos = VoxelTerrain.GetBlockPos(hit);
            //    if (Input.GetMouseButtonDown(0))
            //        VoxelTerrain.SetBlock(hit, BlockCollection.Blocks[0]);
            //    if (Input.GetMouseButtonDown(1))
            //        VoxelTerrain.SetBlock(hit, blocks[selectedBlock], true);
            //}
        }

        Vector3 GetHitPosition()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                return hit.point + hit.normal;
            }

            return Vector3.zero;
        }

        RaycastHit GetMouseHitPosition()
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 100);
            return hit;
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = selectedBlock != 0 ? Color.green : Color.red;

        //    if (dragging)
        //    {
        //        Vector3 pos = new Vector3(dragStart.x + dragEnd.x, dragStart.y + dragEnd.y, dragStart.z + dragEnd.z) / 2;
        //        Vector3 scale = new Vector3(Mathf.Abs(dragStart.x - dragEnd.x) + 1, Mathf.Abs(dragStart.y - dragEnd.y) + 1, Mathf.Abs(dragStart.z - dragEnd.z) + 1);
        //        Gizmos.DrawCube(pos, scale);
        //    }
        //}

        void HandleMouse()
        {
            if (!lookAround)
                return;

            float rotX = Input.GetAxis("Mouse X");
            float rotY = Input.GetAxis("Mouse Y");

            Vector3 eulerRotation = transform.localRotation.eulerAngles;

            eulerRotation.x -= rotY * 4;
            eulerRotation.y += rotX * 4;
            eulerRotation.z = 0;
            transform.localRotation = Quaternion.Euler(eulerRotation);
        }

        void HandleMovement()
        {
            if (!lookAround)
                return;

            transform.position += transform.forward * 10 * Input.GetAxisRaw("Vertical") * Time.deltaTime;
            transform.position += transform.right * 10 * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        }

        void OnPreRender()
        {
            GL.wireframe = enableWireframe;
        }
        void OnPostRender()
        {
            GL.wireframe = false;
        }

        private const int GUI_SIZE = 70;
        string command = "";

        private void OnGUI()
        {
            overGUI = Event.current.type == EventType.Repaint && new Rect(0, Screen.height - GUI_SIZE, Screen.width, GUI_SIZE).Contains(Event.current.mousePosition);

            command = GUI.TextField(new Rect(0, 0, 300, 30), command);
            if (GUI.Button(new Rect(300, 0, 100, 30), "Execute"))
            {
                DoCommand(command);
                command = "";
            }

            //GUI.Box(new Rect(0, 30, 150, 20), $"Looking at {lookingAtBlockPos.x} {lookingAtBlockPos.y} {lookingAtBlockPos.z}");

            GUILayout.BeginArea(new Rect(0, Screen.height - GUI_SIZE, Screen.width, GUI_SIZE), GUI.skin.box);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Dig", GUILayout.Width(GUI_SIZE - 10), GUILayout.Height(GUI_SIZE - 10)))
                selectedBlock = "air";
            for (int i = 0; i < blockCollection.Blocks.Length; i++)
            {
                if (GUILayout.Button(blockCollection.Blocks[i].BlockName, GUILayout.Width(GUI_SIZE - 10), GUILayout.Height(GUI_SIZE - 10)))
                {
                    selectedBlock = blockCollection.Blocks[i].BlockName.ToLower().Replace(" ", "");
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DoCommand(string command)
        {
            string[] commandContent = command.Split(' ');
            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i].CommandName.ToLower() == commandContent[0])
                {
                    string[] arguments = new string[commandContent.Length - 1];
                    for (int j = 1; j < commandContent.Length; j++)
                    {
                        arguments[j - 1] = commandContent[j];
                    }

                    commands[i].Execute(arguments);
                    return;
                }
            }
            //if (commandContent[0] == "setblock")
            //{



            //    return;
            //}
            //else if (commandContent[0] == "fill")
            //{
            //    int x1, x2, y1, y2, z1, z2;

            //    if (!int.TryParse(commandContent[1], out x1))
            //    {
            //        Debug.LogWarning("Can't convert X!");
            //        return;
            //    }

            //    if (!int.TryParse(commandContent[2], out y1))
            //    {
            //        Debug.LogWarning("Can't convert Y!");
            //        return;
            //    }

            //    if (!int.TryParse(commandContent[3], out z1))
            //    {
            //        Debug.LogWarning("Can't convert Z!");
            //        return;
            //    }

            //    if (!int.TryParse(commandContent[4], out x2))
            //    {
            //        Debug.LogWarning("Can't convert X!");
            //        return;
            //    }

            //    if (!int.TryParse(commandContent[5], out y2))
            //    {
            //        Debug.LogWarning("Can't convert Y!");
            //        return;
            //    }

            //    if (!int.TryParse(commandContent[6], out z2))
            //    {
            //        Debug.LogWarning("Can't convert Z!");
            //        return;
            //    }

            //    int block;
            //    if (!int.TryParse(commandContent[7], out block))
            //    {
            //        Debug.LogWarning("Can't convert block!");
            //        return;
            //    }

            //    if (block < 0 || block >= blocks.Length)
            //    {
            //        Debug.LogWarning("Block is out of bounds!");
            //        return;
            //    }

            //    World.Instance.FillBlocks(x1, y1, z1, x2, y2, z2, blocks[block]);
            //    return;
            //}

            Debug.LogWarning("Unknown command!");
        }
    }
}
