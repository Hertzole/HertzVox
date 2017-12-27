using Hertzole.HertzVox.Saving;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.HertzVox.Examples
{
    public class ExampleSaveManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private string m_FileExtension = "hvox";
        public string FileExtension
        {
            get
            {
                string toReturn = m_FileExtension;
                if (!toReturn.StartsWith("."))
                    toReturn = "." + m_FileExtension;

                return toReturn;
            }

            set { m_FileExtension = value; }
        }

        [Header("Windows")]
        [SerializeField]
        private GameObject m_WindowsBackground;
        public GameObject WindowsBackground { get { return m_WindowsBackground; } set { m_WindowsBackground = value; } }
        [SerializeField]
        private GameObject m_SaveWindow;
        public GameObject SaveWindow { get { return m_SaveWindow; } set { m_SaveWindow = value; } }
        [SerializeField]
        private GameObject m_LoadWindow;
        public GameObject LoadWindow { get { return m_LoadWindow; } set { m_LoadWindow = value; } }

        [Header("Save Window")]
        [SerializeField]
        private InputField m_SaveNameField;
        public InputField SaveNameField { get { return m_SaveNameField; } set { m_SaveNameField = value; } }

        [Header("Load Window")]
        [SerializeField]
        private ExampleLoadButton m_LoadButton;
        public ExampleLoadButton LoadButton { get { return m_LoadButton; } set { m_LoadButton = value; } }

        public string SaveLocation { get { return Application.dataPath + "/HertzVox Saves/"; } }

        private List<ExampleLoadButton> m_LoadButtons = new List<ExampleLoadButton>();
        private List<ExampleLoadButton> m_LoadButtonsPool = new List<ExampleLoadButton>();

        private string[] m_SavedWorlds;

        private void Start()
        {
            CloseWindow();
            SaveWindow.SetActive(false);
            LoadWindow.SetActive(false);
            LoadButton.gameObject.SetActive(false);
        }

        public void OpenSaveWindow()
        {
            WindowsBackground.SetActive(true);
            SaveWindow.SetActive(true);
            LoadWindow.SetActive(false);
        }

        public void OpenLoadWindow()
        {
            WindowsBackground.SetActive(true);
            SaveWindow.SetActive(false);
            LoadWindow.SetActive(true);

            MakeLoadButtons();
        }

        private void MakeLoadButtons()
        {
            m_SavedWorlds = Directory.GetFiles(SaveLocation, "*" + FileExtension);
            for (int i = 0; i < m_SavedWorlds.Length; i++)
            {
                ExampleLoadButton button = GetLoadButton();
                button.LoadPath = m_SavedWorlds[i];
                button.ButtonComp.onClick.AddListener(delegate { Load(button.LoadPath); });
            }
        }

        private ExampleLoadButton GetLoadButton()
        {
            ExampleLoadButton newButton = null;

            if (m_LoadButtonsPool.Count > 0)
            {
                newButton = m_LoadButtonsPool[0];
                m_LoadButtonsPool.RemoveAt(0);
            }
            else
            {
                newButton = Instantiate(LoadButton, LoadButton.transform.parent);
            }

            newButton.gameObject.SetActive(true);
            m_LoadButtons.Add(newButton);
            return newButton;
        }

        public void CloseWindow()
        {
            WindowsBackground.SetActive(false);
            for (int i = 0; i < m_LoadButtons.Count; i++)
            {
                m_LoadButtons[i].ButtonComp.onClick.RemoveAllListeners();
                m_LoadButtons[i].gameObject.SetActive(false);
                m_LoadButtonsPool.Add(m_LoadButtons[i]);
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(SaveNameField.text))
            {
                Debug.LogWarning("Cannot save with empty name.");
                return;
            }

            VoxSave save = VoxSaveManager.GetSaveData(World.Instance, true);

            FileStream stream = new FileStream(SaveLocation + SaveNameField.text + FileExtension, FileMode.Create, FileAccess.Write, FileShare.None);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, save);
            stream.Close();

            CloseWindow();
        }

        public void Load(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            try
            {
                //Thread thread = new Thread(() =>
                //{
                //    // Using .NET 4.6 due to this creating huge GC spike otherwise.
                //    BinaryFormatter formatter = new BinaryFormatter();
                //    var data = formatter.Deserialize(stream);
                //    Load((VoxSave)data);
                //});
                //thread.Start();
                BinaryFormatter formatter = new BinaryFormatter();
                var data = formatter.Deserialize(stream);
                Load((VoxSave)data);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Error when loading file: " + ex);
            }
        }

        public void Load(VoxSave save)
        {
            VoxSaveManager.ApplySaveData(save, World.Instance);

            CloseWindow();
        }
    }
}
