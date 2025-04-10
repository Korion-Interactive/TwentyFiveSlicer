using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[assembly: InternalsVisibleTo("TwentyFiveSlicer.Editor")]
namespace TwentyFiveSlicer.Runtime
{
    internal class SliceDataManager
    {
        private static SliceDataManager _instance;
        private SliceDataMap _sliceDataMap;

        public static SliceDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SliceDataManager();
                    _instance.Initialize();
                }
                
                if (_instance._sliceDataMap == null)
                {
                    _instance.Initialize();
                }
                
                return _instance;
            }
        }

        private void Initialize()
        {
            // SliceDataMap ScriptableObject 로드
            _sliceDataMap = SliceDataMap.Instance;
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap not found. Please create a Slice Data Map asset and assign in the project settings under Project/TwentyFiveSlice");
            }
        }

        public bool TryGetSliceData(string guid, out TwentyFiveSliceData data)
        {
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap is not initialized.");
                data = null;
                return false;
            }
            return _sliceDataMap.TryGetSliceData(guid, out data);
        }

#if UNITY_EDITOR
        public void SaveSliceData(string guid, TwentyFiveSliceData sliceData)
        {
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap is not initialized.");
                return;
            }

            _sliceDataMap.AddSliceData(guid, sliceData);
            UnityEditor.EditorUtility.SetDirty(_sliceDataMap);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        
        public bool IsSliceDataMapExist()
        {
            return _sliceDataMap != null;
        }
        
        public IEnumerable<KeyValuePair<string, TwentyFiveSliceData>> GetAllEntries()
        {
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap is not initialized.");
                return Enumerable.Empty<KeyValuePair<string, TwentyFiveSliceData>>();
            }

            return _sliceDataMap.GetAllEntries();
        }

        public void RemoveSliceData(string guid)
        {
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap is not initialized.");
                return;
            }

            _sliceDataMap.RemoveSliceData(guid);
            UnityEditor.EditorUtility.SetDirty(_sliceDataMap);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}