﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Presenter
{
    private Button.Factory _factoryButton;
    private GameSettings _settings;
    private GameController _gameController;
    private SignalBus _signalbus;
    
    private List<Button> _buttons = new List<Button>();

    public Presenter(
        GameSettings settings, 
        Button.Factory factoryButton, 
        GameController gameController,
        SignalBus signalBus)
    {
        _settings = settings;
        _factoryButton = factoryButton;
        _gameController = gameController;
        _signalbus = signalBus;
        
        _signalbus.Subscribe<CreateViewObjectSignal>(NewViewObject);
        _signalbus.Subscribe<DespawnedViewObjectSignal>(DespawnViewObject);
    }

    public void CreateButtons()
    {
        foreach (GameSettings.ContainerObjectDetails param in _settings.ObjectDetails)
        {
            var button  = _factoryButton.Create(param, _gameController.Content.transform);
            button.UpdateCount();
            _buttons.Add(button);
        }
    }

    private void NewViewObject(CreateViewObjectSignal arg)
    {
        var button = _buttons.Find(b => b.Name == arg.Name);
        button.ModelButton.Change(+1);
        button.UpdateCount();
    }

    private void DespawnViewObject(DespawnedViewObjectSignal arg)
    {
        var button = _buttons.Find(b => b.Name == arg.Name);
        button.ModelButton.Change(-1);
        button.UpdateCount();
    }
    public class Factory : PlaceholderFactory<Presenter>
    {
    }
}