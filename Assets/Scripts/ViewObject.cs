using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ViewObject : MonoBehaviour, IPoolable<GameSettings.ContainerObjectDetails, IMemoryPool>
{
    [SerializeField] private MeshFilter _meshFilter;
    
    [Inject] private GameSettings _settings;
    [Inject] private SignalBus _signalBus;
    
    private IMemoryPool _pool;
    private bool _isCenter = false;

    private string _name = null;
    [SerializeField] private MeshRenderer _renderer = null;

    private void Start()
    {
        //_renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        transform.Translate(Vector3.right * (_settings.SpeedViewObject * Time.deltaTime));
        
        if (transform.position.x > -_settings.StartPosition.x)
        {
            _pool.Despawn(this);
        }
        else if (transform.position.x > _settings.PointNewSpawnerViewObject && !_isCenter)
        {
            _isCenter = true;
            _signalBus.Fire<ViewObjectInCenterSignal>();
        }
    }

    public void OnDespawned()
    {
        _pool = null;
        _isCenter = false;
        transform.position = _settings.StartPosition;
        _signalBus.Fire(new DespawnedViewObjectSignal() {Name = _name});
    }

    public void OnSpawned(GameSettings.ContainerObjectDetails p1, IMemoryPool p2)
    {
        _meshFilter.mesh = p1.Mesh;
        _name = p1.Name;
        _pool = p2;

        _renderer.material.color = p1.Color;

        transform.position = _settings.StartPosition;
        
        _signalBus.Fire(new CreateViewObjectSignal() {Name = _name});
    }
    
    public class Factory : PlaceholderFactory<GameSettings.ContainerObjectDetails, ViewObject>
    {
    }
}
