using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private GameSettings _settings;
    [Inject] private Presenter.Factory _factoryPresenter;
    [Inject] private SignalBus _signalBus;
    
    [SerializeField] private GameObject _content;
    public GameObject Content { get; private set; }

    void Start()
    {
        Content = _content;
        
        var presenter = _factoryPresenter.Create();
        presenter.CreateButtons();
        
        // foreach (GameSettings.ContainerObjectDetails objectDetail in _settings.ObjectDetails)
        // {
        //     _factoryButton.Create(objectDetail, _content.transform);
        // }
    }
}
