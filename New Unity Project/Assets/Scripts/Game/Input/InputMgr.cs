using System;
using System.Collections.Generic;
using UnityEngine;
public class InputMgr : Singleton<InputMgr>
{
    public InputController controller;
    public InputController lastController;
    public void SwichScene<T>(object data = null) where T : new()
    {
        if (controller != null)
        {
            lastController = controller;
        }
        controller = null;
        controller = new T() as InputController;;
    }
    public void ResetLastController()
    {
        controller = lastController;
    }
    public void Update(float dt)
    {
        if (controller != null)
        {
            controller.Update(dt);
        }
    }
}
