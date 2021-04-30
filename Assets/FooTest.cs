using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FooTest : MonoBehaviour
{
    public class Factory : PlaceholderFactory<GameObject, FooTest>
    { }
}

public class FooTestFactory : IFactory<GameObject, FooTest>
{
    private readonly DiContainer _container;

    public FooTestFactory(DiContainer container)
    {
        _container = container;
    }

    public FooTest Create(GameObject prefab)
    {
        return _container.InstantiatePrefabForComponent<FooTest>(prefab);
    }
}
