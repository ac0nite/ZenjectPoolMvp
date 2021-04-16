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
        public Color Color;
    }
    
    [Serializable]
    public struct PreviewSprite
    {
        public string Name;
        public Sprite Preview;
    }
    
    [Serializable]
    public struct MeshObject
    {
        public string Name;
        public Mesh Mesh;
    }

    public List<ContainerObjectDetails> ObjectDetails;
    
    [Header("Словари соответсвия")]
    [SerializeField] private List<PreviewSprite> _containerPreviewSprites;
    [SerializeField] private List<MeshObject> _containerMeshObjects;
    public readonly Dictionary<string, Sprite> ContainerPreviewSprites = new Dictionary<string, Sprite>();
    public readonly Dictionary<string, Mesh> ContainerMeshObjects = new Dictionary<string, Mesh>();
    
    
    [Header("Префабы")]
    public GameObject ButtonPrefab;
    public GameObject ViewObjectPrefab;
    
    [Header("Настройки")]
    public Vector3 StartPosition;
    public float SpeedViewObject;
    public float PointNewSpawnerViewObject;

    [Header("Сетевые ресурсы")]
    public string URL;
    public string URLBundlePrefabs;

    //необходимо вызывать в самом начале запуска
    public void InitDictionaryObjectDetails()
    {
        foreach (PreviewSprite containerPreviewSprite in _containerPreviewSprites)
        {
            ContainerPreviewSprites[containerPreviewSprite.Name] = containerPreviewSprite.Preview;
        }

        foreach (MeshObject containerMeshObject in _containerMeshObjects)
        {
            ContainerMeshObjects[containerMeshObject.Name] = containerMeshObject.Mesh;
        }
    }
}
