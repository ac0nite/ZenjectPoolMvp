using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem(itemName: "Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles(
            "Assets/AssetBundle", 
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64);
    }
}
