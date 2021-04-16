using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Networking;

public class LoadData : MonoBehaviour
{
    void Start()
    {
        //LoadFile();
        //StartCoroutine(LoadDataAsyn());

        //StartCoroutine(LoadDataAsyn(Path.Combine(Application.dataPath, "AssetBundle", "test", "buttons"), SuccesLoadButtons));
        //StartCoroutine(LoadDataAsyn(Path.Combine(Application.dataPath, "AssetBundle", "test", "prefabs"), SuccesLoadPrefabs));

        //StartCoroutine(LoadBundle("https://drive.google.com/uc?export=download&id=1fty6PDIlkE4MWeA81-xl1xKxWMH6NywN", SuccesLoadPrefabs, ErrorLoad));

        //StartCoroutine(LoadDataAsyn(Path.Combine(Application.dataPath, "AssetBundle", "AssetBundle"), SuccesLoadManifest));

    }

    private void SuccesLoadButtons(AssetBundle bundle)
    {
        Debug.Log($"SuccesLoadButtons {bundle}");
    }

    private void SuccesLoadPrefabs(AssetBundle bundle)
    {
        Debug.Log($"SuccesLoadPrefabs {bundle}");
    }

    private void ErrorLoad(string error)
    {
        Debug.LogError(error);
    }

    private void SuccesLoadManifest(AssetBundle bundle)
    {
        Debug.Log($"SuccesLoadManifest {bundle}");
        AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] depencies = manifest.GetAllAssetBundles();
        Debug.Log($"depencies: {depencies.Length}");
        foreach (string depency in depencies)
        {
            Debug.Log($"depency: {depency}");
        }
    }

    private static IEnumerator LoadBundle(string url, Action<AssetBundle> successCallback, Action<string> errorCallback)
    {
        var request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            errorCallback?.Invoke(request.error);
        }
        else
        {
            AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
            successCallback?.Invoke(bundle);
        }
    }

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

