using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEditor;
using TwentyFiveSlicer.Runtime;

namespace TwentyFiveSlicer.TFSEditor
{
    public class TwentyFiveSliceBuildPlayer : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            SliceDataMap settings = TwentyFiveSliceDataMapProvider.DataMap;
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();
            ArrayUtility.Remove(ref preloadedAssets, settings);
            ArrayUtility.Add(ref preloadedAssets, settings);
            PlayerSettings.SetPreloadedAssets(preloadedAssets);
        }
    }
}