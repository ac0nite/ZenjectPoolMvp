using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject objectOne = null;
    [SerializeField] private GameObject objectTwo = null;
    [SerializeField] private Material sharedMaterial = null;

    [SerializeField] private Color color = Color.black;

    private MeshRenderer renderer = null;

    // Start is called before the first frame update
    void Start()
    {
        objectOne.GetComponent<MeshRenderer>().material = sharedMaterial;
        objectTwo.GetComponent<MeshRenderer>().material = sharedMaterial;

        renderer = objectOne.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //renderer.sharedMaterial.color = color;
        renderer.material.color = color;
    }
}
