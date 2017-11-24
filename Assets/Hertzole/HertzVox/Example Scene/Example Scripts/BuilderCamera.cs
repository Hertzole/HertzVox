using Hertzole.HertzVox.Blocks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hertzole.HertzVox
{
    public class BuilderCamera : MonoBehaviour
    {
        [SerializeField]
        private float m_MoveSpeed = 8;
        public float MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

        [SerializeField]
        private BlockCollection m_BlockCollection;
        public BlockCollection BlockCollection { get { return m_BlockCollection; } set { m_BlockCollection = value; } }
        [SerializeField]
        private Transform m_PlacementCube;
        public Transform PlacementCube { get { return m_PlacementCube; } set { m_PlacementCube = value; } }
        [SerializeField]
        private Material m_AddMaterial;
        public Material AddMaterial { get { return m_AddMaterial; } set { m_AddMaterial = value; } }
        [SerializeField]
        private Material m_SubtractMaterial;
        public Material SubtractMaterial { get { return m_SubtractMaterial; } set { m_SubtractMaterial = value; } }
        [SerializeField]
        private Button m_BlockButton;
        public Button BlockButton { get { return m_BlockButton; } set { m_BlockButton = value; } }
        [SerializeField]
        private Text m_SelectedBlockText;
        public Text SelectedBlockText { get { return m_SelectedBlockText; } set { m_SelectedBlockText = value; } }

        private string m_SelectedBlock = "air";

        private bool m_EnableWireframe = false;
        private bool m_LookAround = true;
        private bool m_Dragging = false;

        private Camera m_Cam;

        private RaycastHit m_Hit;
        private EventSystem m_CurrentEventSystem;

        private BlockPos m_DragStart;
        private BlockPos m_DragEnd;

        private Renderer m_PlacementCubeRenderer;

        // Use this for initialization
        void Start()
        {
            m_Cam = GetComponent<Camera>();
            m_CurrentEventSystem = EventSystem.current;
            SetupPlacementCube();
            SetupUI();
        }

        private void SetupUI()
        {
            BlockButton.gameObject.SetActive(false);

            Button digButton = Instantiate(BlockButton, BlockButton.transform.parent);
            digButton.GetComponentInChildren<Text>().text = "Dig";
            digButton.gameObject.name = "Dig";
            digButton.gameObject.SetActive(true);
            digButton.onClick.AddListener(delegate { SetBlock("air"); });

            for (int i = 0; i < BlockCollection.Blocks.Length; i++)
            {
                var block = BlockCollection.Blocks[i];
                Button newButton = Instantiate(BlockButton, BlockButton.transform.parent);
                newButton.GetComponentInChildren<Text>().text = block.BlockName;
                newButton.gameObject.name = block.BlockName;
                newButton.gameObject.SetActive(true);
                newButton.onClick.AddListener(delegate { SetBlock(block.BlockName); });
            }

            SetBlock("air");
        }

        private void SetupPlacementCube()
        {
            if (!m_PlacementCube)
                return;

            m_PlacementCubeRenderer = m_PlacementCube.GetComponent<Renderer>();
            m_PlacementCubeRenderer.material = m_AddMaterial;
        }

        public void SetBlock(string name)
        {
            m_SelectedBlock = name;
            if (SelectedBlockText != null)
                SelectedBlockText.text = name;

            if (m_PlacementCubeRenderer)
            {
                if (name == "air")
                    m_PlacementCubeRenderer.material = m_SubtractMaterial;
                else
                    m_PlacementCubeRenderer.material = m_AddMaterial;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_CurrentEventSystem.currentSelectedGameObject != null && m_CurrentEventSystem.currentSelectedGameObject.GetComponent<InputField>())
                    return;

                m_EnableWireframe = !m_EnableWireframe;
                m_Cam.clearFlags = m_EnableWireframe ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
            }

            if (!m_CurrentEventSystem.IsPointerOverGameObject())
            {
                m_LookAround = Input.GetMouseButton(1);
                Cursor.lockState = m_LookAround ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !m_LookAround;
            }
            else
            {
                m_LookAround = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            HandleBuilding();
            HandleMouse();
            HandleMovement();
        }

        void HandleBuilding()
        {
            if (m_CurrentEventSystem.IsPointerOverGameObject())
            {
                m_PlacementCube.gameObject.SetActive(false);
                return;
            }

            if (m_CurrentEventSystem.IsPointerOverGameObject())
                return;

            m_Hit = GetMouseHitPosition();
            bool adjacent = true;
            if (((Block)m_SelectedBlock).type == Block.Air.type)
                adjacent = false;

            DrawBlockCursor(m_Hit, adjacent);

            if (m_LookAround)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                //dragging = true;

                VoxelTerrain.SetBlock(m_Hit, m_SelectedBlock, adjacent);
                //dragStart = VoxelTerrain.GetBlockPos(GetMouseHitPosition(), adjacent);
            }

            //if (Input.GetMouseButton(0) && dragging)
            //    dragEnd = GetMouseHitPosition().point;

            //if (Input.GetMouseButtonUp(0) && dragging)
            //{
            //    dragging = false;
            //    dragEnd = GetMouseHitPosition().point;

            //    VoxelTerrain.FillBlocks(dragStart, dragEnd, selectedBlock);
            //}
        }

        private void DrawBlockCursor(RaycastHit hit, bool adjacent)
        {
            if (m_LookAround)
            {
                m_PlacementCube.gameObject.SetActive(false);
                return;
            }

            if (hit.transform != null)
            {
                m_PlacementCube.gameObject.SetActive(true);
                Vector3 position = VoxelTerrain.GetBlockPos(hit, adjacent);
                m_PlacementCube.position = position;
            }
            else
                m_PlacementCube.gameObject.SetActive(false);

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
            Ray ray = m_Cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 100);
            return hit;
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = selectedBlock != "air" ? Color.green : Color.red;

        //    if (dragging)
        //    {
        //        Vector3 pos = new Vector3(dragStart.x + dragEnd.x, dragStart.y + dragEnd.y, dragStart.z + dragEnd.z) / 2;
        //        Vector3 scale = new Vector3(Mathf.Abs(dragStart.x - dragEnd.x) + 1, Mathf.Abs(dragStart.y - dragEnd.y) + 1, Mathf.Abs(dragStart.z - dragEnd.z) + 1);
        //        Gizmos.DrawCube(pos, scale);
        //    }
        //}

        void HandleMouse()
        {
            if (!m_LookAround)
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
            if (!m_LookAround)
                return;

            transform.position += transform.forward * MoveSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime;
            transform.position += transform.right * MoveSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        }

        void OnPreRender()
        {
            GL.wireframe = m_EnableWireframe;
        }

        void OnPostRender()
        {
            GL.wireframe = false;
        }
    }
}
