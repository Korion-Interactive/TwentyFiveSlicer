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
        private SerializedDictionary<Sprite, TwentyFiveSliceData> sliceDataMap = new();

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

        public bool TryGetSliceData(Sprite sprite, out TwentyFiveSliceData data)
        {
            return sliceDataMap.TryGetValue(sprite, out data);
        }

        public void AddSliceData(Sprite sprite, TwentyFiveSliceData data)
        {
            sliceDataMap.Add(sprite, data);
        }

        public void RemoveSliceData(Sprite sprite)
        {
            sliceDataMap.Remove(sprite);
        }

        public IEnumerable<Sprite> GetAllSprites()
        {
            return sliceDataMap.Keys;
        }
        
        public IEnumerable<KeyValuePair<Sprite, TwentyFiveSliceData>> GetAllEntries()
        {
            return sliceDataMap.GetAllEntries();
        }
    }
}