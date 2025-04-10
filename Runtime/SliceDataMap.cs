using UnityEngine;
using System.Collections.Generic;
using Twentyfiveslicer.Runtime.SerializedDictionary;

namespace TwentyFiveSlicer.Runtime
{
    [CreateAssetMenu(fileName = "SliceDataMap", menuName = "TwentyFiveSlicer/SliceDataMap", order = 0)]
    public class SliceDataMap : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<string, TwentyFiveSliceData> sliceDataMap = new();

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