using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    void Start()
    {
        //LoadFile();
        StartCoroutine(LoadDataAsyn());
    }

    //https://docs.unity3d.com/Manual/AssetBundles-Dependencies.html

    private IEnumerator LoadDataAsyn()
    {
        var request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(Path.Combine(Application.dataPath, "AssetBundle", "test", "buttons"), 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);

        Debug.Log($"mesh: {bundle.LoadAsset("cube")}");
        Debug.Log($"music: {bundle.LoadAsset("din2")}");
    }

    private void LoadFile()
    {
        Debug.Log("--------");
        Debug.Log(Application.dataPath);
        Debug.Log(Application.streamingAssetsPath);

        Debug.Log(Path.Combine(Application.dataPath, "AssetBundle", "test", "buttons"));
        Debug.Log("--------");

        //var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "myassetBundle"));

        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundle", "test", "buttons"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        else
        {
            Debug.Log($"myLoadedAssetBundle: {myLoadedAssetBundle}");
            var p = myLoadedAssetBundle.LoadAsset("din");
            Debug.Log($"music: {p}");
            Debug.Log($"mesh: {myLoadedAssetBundle.LoadAsset("cube")}");

        }
        //var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("MyObject");
        //Instantiate(prefab);
    }
}

