using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    public int Count { get; private set; }
    public event Action<int> EventChangeCount; 

    public Model()
    {
        Count = 0;
    }
    
    public void Change(int value)
    {
        Count += value;
        EventChangeCount?.Invoke(Count);
    }
}
