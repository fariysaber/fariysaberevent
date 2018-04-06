using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;

public class UBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Assembly ass = Assembly.GetExecutingAssembly();
        return ass.GetType(typeName);
    }
}
