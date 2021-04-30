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
    [Inject] private Presenter.Factory _factoryPresenter = null;
    [Inject] private SignalBus _signalBus;
    [Inject] private NetworkManager _networkManager;
    
    [SerializeField] private GameObject _content;
    public GameObject Content { get; private set; }

    [Inject] private ViewObject.Factory _viewObjectFactory;

    //[Inject] private FooTest.Factory _factoryFoo;
    [Inject] private PoolManager _pool = null;

    //[SerializeField] private GameObject _testPrefab;

    void Start()
    {
        Content = _content;
        _settings.InitDictionaryObjectDetails();

        // _factoryFoo.Create(_testPrefab).transform.gameObject
        // pool.Init<GameObject, FooTest.Factory>(_testPrefab, _factoryFoo);

        //pool.Init("test", _testPrefab, 20);
        //FooTest obj = _factoryFoo.Create(_testPrefab);

        //pool.Init<FooTest, FooTest.Factory>("test", _testPrefab, _factoryFoo, 10);

        //pool.Init<GameObject, FooTest, FooTest.Factory>("test", _testPrefab, _factoryFoo, 20);

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
        _settings.ViewObjectPrefab = bundle.LoadAsset<GameObject>("ViewObject");
        _settings.ButtonPrefab = bundle.LoadAsset<GameObject>("ButtonPrefab");

        Debug.Log($"LoadBundle: {_settings.ViewObjectPrefab} {_settings.ButtonPrefab}");

        bundle.Unload(false);

        var presenter = _factoryPresenter.Create();

        presenter.CreateButtons();

        _pool.Init<ViewObject, ViewObject.Factory>("ViewObject", _settings.ViewObjectPrefab, _viewObjectFactory, 10);
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
