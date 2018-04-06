using System;
using System.Collections.Generic;
using System.ComponentModel;

[Serializable]
public class ModelInfo
{
    [Description("模型名字")]
    public string name;
    [Description("模型路径")]
    public string path;

    public int colliderType;
    public List<float> colliderData;

    public bool hasController;
    public List<float> cotrollerData;
}
