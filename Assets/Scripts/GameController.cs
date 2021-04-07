using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private GameSettings _settings;
    [Inject] private Presenter.Factory _factoryPresenter;
    [Inject] private SignalBus _signalBus;
    [Inject] private NetworkManager _networkManager;
    
    [SerializeField] private GameObject _content;
    public GameObject Content { get; private set; }

    void Start()
    {
        Content = _content;
        _settings.InitDictionaryObjectDetails();
        
        //_networkManager.EventResponse += OnResponse;
        //_networkManager.Get(_currentType = NetworkManager.TypeRequest.GET_INIT_DATA);
        //_networkManager.Post(_currentType = NetworkManager.TypeRequest.ADD_INIT_DATA);

        //var presenter = _factoryPresenter.Create();
        //presenter.CreateButtons();

        // foreach (GameSettings.ContainerObjectDetails objectDetail in _settings.ObjectDetails)
        // {
        //     _factoryButton.Create(objectDetail, _content.transform);
        // }

        StartCoroutine(NetworkManager.LoadTextByUrl(_settings.URL, LoadData, Error));
    }

    private void LoadData(string jsonString)
    {
        //Debug.Log(jsonString);
        List<Data> arr = JsonConvert.DeserializeObject<List<Data>>(jsonString);

        foreach (Data data in arr)
        {
            if (!_settings.ContainerMeshObjects.ContainsKey(data.meshName) ||
                !_settings.ContainerPreviewSprites.ContainsKey(data.previewName))
            {
                Debug.LogError($"Not found key: {data.meshName} or {data.previewName} in name: {data.name}");
            }
            else
            {
                _settings.ObjectDetails.Add(new GameSettings.ContainerObjectDetails()
                {
                    Name = data.name,
                    Mesh = _settings.ContainerMeshObjects[data.meshName],
                    Preview = _settings.ContainerPreviewSprites[data.previewName]
                });
            }
        }

        //Debug.Log(_settings.ObjectDetails.Count);
        var presenter = _factoryPresenter.Create();
        presenter.CreateButtons();
    }

    private void Error()
    {
        Debug.LogError("Error load data");
    }

    //private void OnResponse(NetworkManager.TypeRequest type)
    //{
    //    if (_currentType != type)
    //        return;
        
    //    if (_networkManager.Request.isNetworkError)
    //    {
    //        Debug.LogWarning($"{_networkManager.Request.error}. Code:{_networkManager.Request.isHttpError} ");
    //    }
    //    else
    //    {
    //        var arr = _networkManager.GetResponseDataInit();
    //        _settings.ObjectDetails.Clear();
    //        foreach (Data data in arr)
    //        {
    //            if (!_settings.ContainerMeshObjects.ContainsKey(data.meshName) ||
    //                !_settings.ContainerPreviewSprites.ContainsKey(data.previewName))
    //            {
    //                Debug.LogError($"Not found key: {data.meshName} or {data.previewName} in name: {data.name}");
    //            }
    //            else
    //            {
    //                _settings.ObjectDetails.Add(new GameSettings.ContainerObjectDetails()
    //                {
    //                    Name = data.name,
    //                    Mesh = _settings.ContainerMeshObjects[data.meshName],
    //                    Preview = _settings.ContainerPreviewSprites[data.previewName]
    //                });   
    //            }
    //        }
            
    //        //Debug.Log(_settings.ObjectDetails.Count);
    //        var presenter = _factoryPresenter.Create();
    //        presenter.CreateButtons();
    //    }
    //}

    private void OnDestroy()
    {
        //_networkManager.EventResponse -= OnResponse;
        _settings.ObjectDetails.Clear();
    }
}
