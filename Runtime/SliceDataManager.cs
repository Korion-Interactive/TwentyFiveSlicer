using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            _sliceDataMap = Resources.Load<SliceDataMap>("SliceDataMap");
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap not found. Please create and place it in the Resources folder. Its name should be 'SliceDataMap'.");
            }
        }

        public bool TryGetSliceData(Sprite sprite, out TwentyFiveSliceData data)
        {
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap is not initialized.");
                data = null;
                return false;
            }
            return _sliceDataMap.TryGetSliceData(sprite, out data);
        }

#if UNITY_EDITOR
        public void SaveSliceData(Sprite targetSprite, TwentyFiveSliceData sliceData)
        {
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap is not initialized.");
                return;
            }
            _sliceDataMap.AddSliceData(targetSprite, sliceData);
            UnityEditor.EditorUtility.SetDirty(_sliceDataMap);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        
        public bool IsSliceDataMapExist()
        {
            return _sliceDataMap != null;
        }
        
        public IEnumerable<KeyValuePair<Sprite, TwentyFiveSliceData>> GetAllEntries()
        {
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap is not initialized.");
                return Enumerable.Empty<KeyValuePair<Sprite, TwentyFiveSliceData>>();
            }

            return _sliceDataMap.GetAllEntries();
        }

        public void RemoveSliceData(Sprite sprite)
        {
            if (_sliceDataMap == null)
            {
                Debug.LogError("SliceDataMap is not initialized.");
                return;
            }

            _sliceDataMap.RemoveSliceData(sprite);
            UnityEditor.EditorUtility.SetDirty(_sliceDataMap);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}