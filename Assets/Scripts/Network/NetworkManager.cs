using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class NetworkManager : MonoBehaviour
{
    public UnityWebRequest Request { get; private set; }
    private GameSettings _settings;

    [Inject]
    public void Construct(GameSettings settings)
    {
        _settings = settings;
    }

    public static IEnumerator LoadTextByUrl(string url, Action<string> callback, Action errorCallback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (!request.isHttpError && !request.isNetworkError)
        {
            callback.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Error url: {url} message: {request.error}");
            errorCallback?.Invoke();
        }
    }

    public void Post()
    {
        StartCoroutine(SendPostDataInit(_settings.URL));
    }

    IEnumerator SendPostDataInit(string url)
    {
        WWWForm formData = new WWWForm();
        Data data = new Data()
        {
            name = "testName",
            meshName = "testMeshName",
            previewName = "testPreviewName"
        };

        string json = JsonConvert.SerializeObject(data);
        UnityWebRequest request = UnityWebRequest.Post(url, json);
        byte[] postBytes = Encoding.UTF8.GetBytes(json);
        UploadHandler uploadHandler = new UploadHandlerRaw(postBytes);
        request.uploadHandler = uploadHandler;
        request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");

        yield return request.SendWebRequest();

       // _dataInit.Clear();
        Debug.Log($"Error: {request.error}");
        Debug.Log(request.downloadHandler.text);

        //if (!Request.isNetworkError)
        //{
        //    _dataInit = JsonConvert.DeserializeObject<List<Data>>(Request.downloadHandler.text);
        //}
        //EventResponse?.Invoke(type);
    }
}
