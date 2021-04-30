using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class PoolManager // : SingletoneGameObject<PoolManager>
{
    [System.Serializable]
    public struct PoolPart
    {
        public string Name;
        public int Count;
        public GameObject Prefab;
        public ObjectPooling PartPool;
    }

    private Dictionary<string, PoolPart> _pools = null;
    //private Dictionary<string, PoolPart> _pools = new Dictionary<string, PoolPart>();

    private GameObject _parent = null;

    public PoolManager()
    {
        _pools = new Dictionary<string, PoolPart>();
        _parent = new GameObject { name = "Pool" };
    }

    //public void Init(PoolPart[] newPools)
    //{
    //    if (_parent == null)
    //    {
    //        _parent = new GameObject { name = "Pool" };
    //    }

    //    for (int i = 0; i < newPools.Length; i++)
    //    {
    //        newPools[i].PartPool = new ObjectPooling(newPools[i].Name, _parent.transform);
    //        newPools[i].PartPool.Init(newPools[i].Count, newPools[i].Prefab);

    //        _pools.Add(newPools[i].Name, newPools[i]);
    //    }
    //}

    public void Init<TObject, TFactory>(string name, GameObject prefab, TFactory factory, int count)
    {
        if (_parent == null)
        {
            _parent = new GameObject { name = "Pool" };
        }

        if (_pools.ContainsKey(name))
        {
            Debug.LogWarning($"Object with name [{name}] exists");
            return;
        }

        //var obj = ((IFactory<GameObject, TObject>) factory).Create(prefab);

       // GameObject _prefab = prefab as GameObject;

        //Func<GameObject, GameObject> ins = o => ((IFactory<GameObject, TObject>) factory).Create(o) as GameObject;

        PoolPart part;

       // GameObject GObject = obj as GameObject;

        
        part.Count = count;
        part.Name = name;
        part.Prefab = prefab;
        part.PartPool = new ObjectPooling(name, _parent.transform);
        part.PartPool._instance = o =>
        {
            var obj = ((IFactory<GameObject, TObject>)factory).Create(o);

            var f = obj as MonoBehaviour;

            //Debug.Log($"Create [{o}] - [{obj}] - [{f.gameObject}]");

            //return obj as GameObject;
            return f.gameObject;
        };
        part.PartPool.Init(count, prefab);

        _pools.Add(name, part);
    }

    public void Init(string name, GameObject prefab, int count = 10)
    {
        if (_parent == null)
        {
            _parent = new GameObject { name = "Pool" };
        }

        if (_pools.ContainsKey(name))
        {
            Debug.LogWarning($"Object with name [{name}] exists");
            return;
        }

        PoolPart part;

        part.Count = count;
        part.Name = name;
        part.Prefab = prefab;
        part.PartPool = new ObjectPooling(name, _parent.transform);
        part.PartPool.Init(count, prefab);

        _pools.Add(name, part);
    }


    public void Despawn(GameObject obj)
    {
        obj.GetComponent<IPoolObject>()?.OnDespawned();
        obj.SetActive(false);

        _pools[obj.name].PartPool._freePoolObjects.Enqueue(obj);
    }

    public int GetCountSpawned()
    {
        int count = 0;
        foreach (var part in _pools)
        {
            count += part.Value.PartPool.Count;
        }
        return count;
    }

    public int GetCountVisibleSpawned()
    {
        int count = 0;
        foreach (var part in _pools)
        {
            count += part.Value.PartPool.CountSpawned;
        }
        return count;
    }

    public int GetCountInVisibleSpawned()
    {
        int count = 0;
        foreach (var part in _pools)
        {
            count += part.Value.PartPool.CountFree;
        }
        return count;
    }

    public GameObject GetObject(string name)
    {
        if (!_pools.ContainsKey(name))
        {
            Debug.LogWarning($"Not found object with name [{name}] in pool");
            return null;
        }

        return _pools[name].PartPool.GetObject().gameObject;
    }

    public GameObject GetObject(string name, Vector3 position, Quaternion quaternion)
    {
        GameObject result = null;

        result = GetObject(name);

        if (result != null)
        {
            result.transform.position = position;
            result.transform.rotation = quaternion;
            result.SetActive(true);
        }
        return result;
    }

    public void Clean(string name)
    {
        if (_pools.ContainsKey(name))
        {
            _pools[name].PartPool.CleanAll();
            _pools.Remove(name);
        }
    }
}
