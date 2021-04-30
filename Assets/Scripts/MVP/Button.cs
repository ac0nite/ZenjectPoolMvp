using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;
using Toggle = UnityEngine.UI.Toggle;

public class Button : MonoBehaviour
{
    [SerializeField] private Text _label = null;
    [SerializeField] private Image _image = null;
    [SerializeField] private Toggle _togle = null;

    [Inject] private SignalBus _signalBus = null;

    public Model ModelButton = null;
    public string Name { get; private set; }

    public void OnTogle()
    {
        if (_togle.isOn)
        {
            _signalBus.Fire(new ChangeTypeViewObjectSignal() { Name = this.Name} );
        }
    }

    private void OnChangeCount(int newValue)
    {
        _label.text = $"x{ModelButton.Count}";
    }

    private void OnDestroy()
    {
        ModelButton.EventChangeCount -= OnChangeCount;
    }

    public void Init(GameSettings.ContainerObjectDetails param1, Transform param2)
    {
        _image.sprite = param1.Preview;
        Name = param1.Name;

        transform.SetParent(param2);

        _togle.group = param2.GetComponent<ToggleGroup>();

        ModelButton = new Model();
        ModelButton.EventChangeCount += OnChangeCount;
        OnChangeCount(ModelButton.Count);
    }

    public class Factory : PlaceholderFactory<Object, GameSettings.ContainerObjectDetails, Transform, Button>
    {
    }
}

public class ButtonFactory : IFactory<Object, GameSettings.ContainerObjectDetails, Transform, Button>
{
    private DiContainer _container;
    public ButtonFactory(DiContainer container)
    {
        _container = container;
    }
    public Button Create(Object param1, GameSettings.ContainerObjectDetails param2, Transform param3)
    {
        var obj = _container.InstantiatePrefabForComponent<Button>(param1);
        obj.Init(param2, param3);
        return obj;
    }
}