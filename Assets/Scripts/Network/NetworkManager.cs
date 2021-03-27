using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class NetworkManager : MonoBehaviour
{
    public UnityWebRequest Request { get; private set; }
    public event Action<TypeRequest> EventResponse;
    
    private List<Data> _dataInit;
    private GameSettings _settings;

    public enum TypeRequest
    {
        UNDEFINED,
        GET_INIT_DATA
        //add...
    }
    
    [Inject]
    public void Construct(GameSettings settings)
    {
        _settings = settings;
    }

    public void Get(TypeRequest type)
    {
        switch (type)
        {
            case TypeRequest.GET_INIT_DATA:
                StartCoroutine(SendRequestDataInit(_settings.URL, type));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    IEnumerator SendRequestDataInit(string url, TypeRequest type)
    {
        Request = UnityWebRequest.Get(url);
        yield return Request.SendWebRequest();
       // callback(Request);
        if (!Request.isNetworkError)
        {
            _dataInit = JsonConvert.DeserializeObject<List<Data>>(Request.downloadHandler.text);
        }
        EventResponse?.Invoke(type);
    }

    public List<Data> GetResponseDataInit() => _dataInit;
}
