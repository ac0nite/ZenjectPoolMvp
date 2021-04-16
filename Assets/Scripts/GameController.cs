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

    [Inject] private FooTest.Factory _factoryFoo;
    [SerializeField] private GameObject _testPrefab;

    void Start()
    {
        Content = _content;
        _settings.InitDictionaryObjectDetails();

        _factoryFoo.Create(_testPrefab, 10);
        

        StartCoroutine(NetworkManager.LoadTextByUrl(_settings.URL, LoadData, Error));
        StartCoroutine(NetworkManager.LoadBundle(_settings.URLBundlePrefabs, LoadBundle, Error));
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
                if (!ColorUtility.TryParseHtmlString(data.colorCode, out Color color))
                {
                    Debug.LogError($"Invalid value color: {data.colorCode}");
                    continue;
                }

                _settings.ObjectDetails.Add(new GameSettings.ContainerObjectDetails()
                {
                    Name = data.name,
                    Mesh = _settings.ContainerMeshObjects[data.meshName],
                    Preview = _settings.ContainerPreviewSprites[data.previewName],
                    Color = color
                });
            }
        }

        //Debug.Log(_settings.ObjectDetails.Count);

        //var presenter = _factoryPresenter.Create();
        //presenter.CreateButtons();
    }

    private void LoadBundle(AssetBundle bundle)
    {
        var viewGameObject = bundle.LoadAsset<GameObject>("ViewObject");
        var buttonPrefab = bundle.LoadAsset<GameObject>("ButtonPrefab");

        //Debug.Log($"LoadBundle: {viewGameObject} {buttonPrefab}");

        _settings.ViewObjectPrefab = bundle.LoadAsset<GameObject>("ViewObject");
        _settings.ButtonPrefab = bundle.LoadAsset<GameObject>("ButtonPrefab");

        bundle.Unload(false);

        var presenter = _factoryPresenter.Create();
        presenter.CreateButtons();
    }

    private void Error()
    {
        Debug.LogError("Error load data");
    }

    private void Error(string error)
    {
        Debug.LogError(error);
    }

    private void OnDestroy()
    {
        _settings.ObjectDetails.Clear();
    }
}
