using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "New game settings")]
public class GameSettings : ScriptableObject
{
    [Serializable]
    public struct ContainerObjectDetails
    {
        public string Name;
        public Sprite Preview;
        public Mesh Mesh;
    }

    public List<ContainerObjectDetails> ObjectDetails;
    
    public GameObject ButtonPrefab;
    
    public GameObject ViewObjectPrefab;
    public Vector3 StartPosition;
    public float SpeedViewObject;
    public float PointNewSpawnerViewObject;
}
