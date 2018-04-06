using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker
{
    private Dictionary<string,bool> _lockerDic;
    public Locker()
    {
        _lockerDic = new Dictionary<string, bool>();
    }
    public void AddLocker(string name)
    {
        _lockerDic[name] = true;
    }
    public void RemoveLocker(string name)
    {
        if (_lockerDic.ContainsKey(name))
        {
            _lockerDic.Remove(name);
        }
    }
    public bool HasLocker()
    {
        return _lockerDic.Count > 0;
    }
}
