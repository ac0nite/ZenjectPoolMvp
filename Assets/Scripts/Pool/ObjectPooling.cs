using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class ObjectPooling
{
    private List<GameObject> _objects = null;
    public Queue<GameObject> _freePoolObjects = new Queue<GameObject>();
    private Transform _parent;
    private int _countInitObjects = 0;
    private int _countReserveObjects = 5;
    public Func<GameObject, GameObject> _instance = null;

    private int CountReserveObjects
    {
        get { return _countReserveObjects; }
        set { _countReserveObjects = Mathf.Clamp(value, 1, 40); }
    }

    public int Count => _objects.Count;
    public int CountSpawned => _objects.Count - _freePoolObjects.Count;
    public int CountFree => _freePoolObjects.Count;

    public ObjectPooling(string name, Transform parent)
    {
        var child = new GameObject($"Pool_{name}");
        child.transform.SetParent(parent);
        _parent = child.transform;
    }

    public void Init(int count, GameObject sample)
    {
        _countInitObjects = count;

        if (_objects == null)
        {
            _objects = new List<GameObject>();
        }

        AddObject(sample, count);
    }

    private void AddObject(GameObject prefab, int count = 1)
    {
        if (prefab.GetComponent<IPoolObject>() == null)
        {
            Debug.LogWarning($"Interface <IPoolObject> not implemented in prefab with name [{prefab.name}]");
        }

        for (int i = 0; i < count; i++)
        {
            GameObject tmp;
            if (_instance != null)
            {
                //Debug.Log("_instance(prefab)");
                tmp = _instance(prefab);
            }
            else
            {
                tmp = GameObject.Instantiate(prefab);
            }
            //GameObject tmp = GameObject.Instantiate(prefab);

            tmp.name = prefab.name;
            tmp.transform.SetParent(_parent);
            tmp.SetActive(false);

            _objects.Add(tmp);

            _freePoolObjects.Enqueue(tmp);
        }
    }

    public GameObject GetObject()
    {
        if (_freePoolObjects.Count == 0)
        {
            AddObject(_objects[0], _countReserveObjects);
        }

        var obj = _freePoolObjects.Dequeue();
        obj.GetComponent<IPoolObject>()?.OnSpawned();
        obj.SetActive(true);

        if (CountFree > 2 * _countInitObjects && Count > 3 * _countInitObjects)
        {
            Debug.Log($"Resize pool with name {_objects[0].name} [all {Count}]\\[free {CountFree}]");
            CleanFreePools(_countInitObjects);
        }

        return obj;
    }

    private void CleanFreePools(int count)
    {
        int size = Mathf.Clamp(count, 0, _freePoolObjects.Count);

        for (int i = 0; i < size; i++)
        {
            var o = _freePoolObjects.Dequeue();
            GameObject.Destroy(o);
            _objects.Remove(o);
        }
    }

    public void CleanAll()
    {
        foreach (GameObject o in _objects)
        {
            GameObject.Destroy(o);
        }
        _objects.Clear();

        GameObject.Destroy(_parent.gameObject);
    }
}
