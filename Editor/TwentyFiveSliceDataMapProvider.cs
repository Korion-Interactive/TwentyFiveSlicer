using System.Collections.Generic;
using TwentyFiveSlicer.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TwentyFiveSlicer.TFSEditor
{
    public class TwentyFiveSliceDataMapProvider : SettingsProvider
    {

        public TwentyFiveSliceDataMapProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public static SliceDataMap DataMap
        {
            get
            {
                EditorBuildSettings.TryGetConfigObject(SliceDataMap.DataMapName, out SliceDataMap dataMap);
                return dataMap;
            }
            set
            {
                if (value == null)
                    EditorBuildSettings.RemoveConfigObject(SliceDataMap.DataMapName);
                else
                    EditorBuildSettings.AddConfigObject(SliceDataMap.DataMapName, value, true);
            }
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            if(DataMap == null)
            {
                DisplayNoDataMapGUI(rootElement);
            }
            else
            {
                DisplayDataMapAvailableGUI(rootElement);
            }
        }

        private void DisplayDataMapAvailableGUI(VisualElement rootElement)
        {
            rootElement.Clear();
            ObjectField objectField = new ObjectField("Data Map");
            objectField.style.paddingLeft = 15;
            objectField.value = DataMap;
            objectField.RegisterValueChangedCallback(e =>
            {
                DataMap = e.newValue as SliceDataMap;
                if (DataMap == null)
                    DisplayNoDataMapGUI(rootElement);
                else
                    DisplayDataMapAvailableGUI(rootElement);
            });

            rootElement.Add(objectField);
            rootElement.Add(new InspectorElement(DataMap));
        }

        private void DisplayNoDataMapGUI(VisualElement rootElement)
        {
            rootElement.Clear();
            HelpBox helpBox = new HelpBox("Could not find Slice Data Map. Assign an asset below or create a new asset!", HelpBoxMessageType.Warning);
            rootElement.Add(helpBox);
            VisualElement horizontalGroup = new VisualElement();
            horizontalGroup.style.flexDirection = FlexDirection.Row;
            ObjectField objectField = new ObjectField("Data Map");
            objectField.objectType = typeof(SliceDataMap);
            objectField.style.flexGrow = 1;
            objectField.RegisterValueChangedCallback(e =>
            {
                if (e.newValue != null)
                {
                    DataMap = e.newValue as SliceDataMap;
                    DisplayDataMapAvailableGUI(rootElement);
                }
            });

            Button createButton = new Button();
            createButton.text = "Create";
            createButton.clicked += () =>
            {
                if (CreateSettingsAssetDialog(out SliceDataMap settings))
                {
                    DataMap = settings;
                    DisplayDataMapAvailableGUI(rootElement);
                }
            };
            horizontalGroup.Add(objectField);
            horizontalGroup.Add(createButton);
            rootElement.Add(horizontalGroup);
        }

        private bool CreateSettingsAssetDialog(out SliceDataMap dataMap)
        {
            dataMap = null;
            string path = EditorUtility.SaveFilePanelInProject("New Data Map", "New Slice Data Map", "asset", "Create new Slice Data Map");
            if (string.IsNullOrEmpty(path))
                return false;

            dataMap = UnityEngine.ScriptableObject.CreateInstance<SliceDataMap>();
            AssetDatabase.CreateAsset(dataMap, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return true;
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var provider = new TwentyFiveSliceDataMapProvider("Project/TwentyFiveSlice", SettingsScope.Project)
            {
                label = "Twenty Five Slice"
            };

            return provider;
        }
    }
}
