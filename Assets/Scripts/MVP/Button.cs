using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class Button : MonoBehaviour
{
    [SerializeField] private Text _label = null;
    [SerializeField] private Image _image = null;
    [SerializeField] private Toggle _togle = null;

    [Inject] private SignalBus _signalBus;

    public Model ModelButton = null;
    
    //private string _name = null;
    public string Name { get; private set; }

    private void Awake()
    {
        // _togle = transform.GetComponentInChildren<Toggle>(true);
        // Debug.Log($"togle: {_togle}");
    }

    [Inject]
    public void Construct(GameSettings.ContainerObjectDetails obj, Transform parent)
    {
        _image.sprite = obj.Preview;
        Name = obj.Name;
        
        transform.SetParent(parent);

        _togle.group = parent.GetComponent<ToggleGroup>();
        
        ModelButton = new Model();
    }

    public void OnTogle()
    {
        if (_togle.isOn)
        {
            _signalBus.Fire(new ChangeTypeViewObjectSignal() { Name = this.Name} );
        }
    }

    public void UpdateCount()
    {
        _label.text = $"x{ModelButton.Count}";
    }

    public class Factory : PlaceholderFactory<GameSettings.ContainerObjectDetails, Transform, Button>
    {
    }
}
