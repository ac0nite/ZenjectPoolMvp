using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ViewObjectFactory : IFactory<UnityEngine.Object, ViewObject>
{
    private readonly DiContainer _container;
    public ViewObjectFactory(DiContainer container)
    {
        _container = container;
    }

    public ViewObject Create(Object param)
    {
        return _container.InstantiatePrefabForComponent<ViewObject>(param);
    }
}
