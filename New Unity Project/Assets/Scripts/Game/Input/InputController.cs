using System;
using System.Collections.Generic;
using UnityEngine;
public class InputController
{
    public virtual void Update(float dt)
    {

    }
}
public class BattleController : InputController
{
    public override void Update(float dt)
    {
        GetKeyCodeMove();
        GetKeyCodeJump();
    }
    private void GetKeyCodeJump()
    {
        if (Input.GetKeyDown(KeyInfo.Jump))
        {
            EventDispatcher.TriggerEvent(GlobalEvents.MAINHERO_JUMP);
        }
        if (Input.GetKeyDown(KeyInfo.Skill1))
        {
            EventDispatcher.TriggerEvent(GlobalEvents.MAINHERO_USESKILL1);
        }
        if (Input.GetKeyDown(KeyInfo.Skill2))
        {
            EventDispatcher.TriggerEvent(GlobalEvents.MAINHERO_USESKILL2);
        }
        if (Input.GetKeyDown(KeyInfo.Skill3))
        {
            EventDispatcher.TriggerEvent(GlobalEvents.MAINHERO_USESKILL3);
        }
    }
    private void GetKeyCodeMove()
    {
        Vector3 move = new Vector3();
        if (Input.GetKey(KeyInfo.leftMove))
        {
            move.x -= 1;
        }
        if (Input.GetKey(KeyInfo.rightMove))
        {
            move.x += 1;
        }
        if (Input.GetKey(KeyInfo.forwardMove))
        {
            move.z += 1;
        }
        if (Input.GetKey(KeyInfo.backMove))
        {
            move.z -= 1;
        }
        if (move != Vector3.zero)
        {
            move = move.normalized;
        }
        EventDispatcher.TriggerEvent<Vector3>(GlobalEvents.MAINHERO_MOVE, move);
    }
}

public class KeyInfo
{
    public static KeyCode leftMove = KeyCode.A;
    public static KeyCode rightMove = KeyCode.D;
    public static KeyCode forwardMove = KeyCode.W;
    public static KeyCode backMove = KeyCode.S;

    public static KeyCode Jump = KeyCode.I;
    public static KeyCode Skill1 = KeyCode.J;
    public static KeyCode Skill2 = KeyCode.K;
    public static KeyCode Skill3 = KeyCode.L;
}