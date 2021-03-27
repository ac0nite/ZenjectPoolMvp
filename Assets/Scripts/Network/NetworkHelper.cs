using System;
using System.Collections.Generic;

[Serializable]
public class Data
{
    public Data(string _name, string _preview, string _mesh)
    {
        name = _name;
        previewName = _preview;
        meshName = _mesh;
    }

    public string name;
    public string previewName;
    public string meshName;
}

