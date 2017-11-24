using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.HertzVox.Examples
{
    [RequireComponent(typeof(Button))]
    public class ExampleLoadButton : MonoBehaviour
    {
        private Button m_ButtonComp;
        public Button ButtonComp { get { if (!m_ButtonComp) m_ButtonComp = GetComponent<Button>(); return m_ButtonComp; } }

        private string m_LoadPath = "";
        public string LoadPath { get { return m_LoadPath; } set { m_LoadPath = value; GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(value); } }
    }
}
