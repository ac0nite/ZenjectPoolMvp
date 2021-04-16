using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FooTest : MonoBehaviour, IPoolable<Object, int, IMemoryPool>
{
    [SerializeField] private int _param = 0;
    private IMemoryPool _pool;

    public void OnDespawned()
    {
        _pool = null;
        _param = 0;
    }

    public void OnSpawned(Object p1, int p2, IMemoryPool p3)
    {
        _param = p2;
        _pool = p3;
    }

    public class Factory : PlaceholderFactory<Object, int, FooTest>
    { }
}

public class FooTestFactory : IFactory<Object, int, FooTest>, IFactory<FooTest>
{
    private readonly DiContainer _container;

    public FooTestFactory(DiContainer container)
    {
        _container = container;
    }
    public FooTest Create(Object prefab, int param)
    {
        Debug.Log($"call Create class FooTestFactory");
        return _container.InstantiatePrefabForComponent<FooTest>(prefab);
    }

    public FooTest Create()
    {
        return _container.Instantiate<FooTest>();
    }
}
