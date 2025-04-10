using UnityEngine;
using System.Collections.Generic;
using Twentyfiveslicer.Runtime.SerializedDictionary;

namespace TwentyFiveSlicer.Runtime
{
    [CreateAssetMenu(fileName = "SliceDataMap", menuName = "TwentyFiveSlicer/SliceDataMap", order = 0)]
    public class SliceDataMap : ScriptableObject
    {
        public const string DataMapName = "com.kwanjoong.twentyfiveslicer";

        [SerializeField]
        private SerializedDictionary<string, TwentyFiveSliceData> sliceDataMap = new();

        private static SliceDataMap s_Instance;

        public static SliceDataMap Instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = GetOrCreateDataMap();

                return s_Instance;
            }
        }

        private static SliceDataMap GetOrCreateDataMap()
        {
            if (s_Instance != null)
                return s_Instance;

            var dataMap = GetInstanceDontCreateDefault();
            if (dataMap == null)
            {
                Debug.LogWarning("Could not find Slice Data Map. Creating default Slice Data Map...");
                dataMap = CreateInstance<SliceDataMap>();
                dataMap.name = "Default Slice Data Map";
            }

            return dataMap;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018", Justification = "Cannot be inlined in builds...")]
        private static SliceDataMap GetInstanceDontCreateDefault()
        {
            SliceDataMap dataMap = null;
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject(DataMapName, out dataMap);
#else
            dataMap = FindObjectOfType<SliceDataMap>();
#endif
            return dataMap;
        }

        private void OnEnable()
        {
            if(s_Instance == null)
                s_Instance = this;
        }


        public bool TryGetSliceData(string guid, out TwentyFiveSliceData data)
        {
            if (string.IsNullOrEmpty(guid))
            {
                Debug.LogErrorFormat("Failed to get SliceData because guid was null or empty!");
                data = null;
                return false;
            }

            return sliceDataMap.TryGetValue(guid, out data);
        }

        public void AddSliceData(string guid, TwentyFiveSliceData data)
        {
            sliceDataMap.Add(guid, data);
        }

        public void RemoveSliceData(string guid)
        {
            sliceDataMap.Remove(guid);
        }

        public IEnumerable<string> GetAllSprites()
        {
            return sliceDataMap.Keys;
        }
        
        public IEnumerable<KeyValuePair<string, TwentyFiveSliceData>> GetAllEntries()
        {
            return sliceDataMap.GetAllEntries();
        }
    }
}