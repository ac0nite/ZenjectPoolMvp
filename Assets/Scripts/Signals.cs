using System;
using UnityEngine;

public class ChangeTypeViewObjectSignal
{
    public String Name { get; internal set; }
}

public class ViewObjectInCenterSignal{}

public class CreateViewObjectSignal
{
    public String Name { get; internal set; }
}

public class DespawnedViewObjectSignal
{
    public String Name { get; internal set; }
}