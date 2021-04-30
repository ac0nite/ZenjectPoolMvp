using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    void OnDespawned();
    void OnSpawned();
}

//public interface IPoolObject<T1>
//{
//    void OnDespawned();
//    void OnSpawned(T1 param);
//}

//public interface IPoolObject<T1, T2>
//{
//    void OnDespawned();
//    void OnSpawned(T1 param1, T2 param2);
//}