using System;
using System.Collections.Generic;
using UnityEngine;

public class ActMoveCtrl
{
    public PlayerModel baseMode;
    public ActHitBack actHitBack;
    public DownGround downGround;
    public ActSkillMove actSkillMove; 
    public void Init(PlayerModel mode)
    {
        baseMode = mode;
        actHitBack = new ActHitBack(baseMode);
        downGround = new DownGround(baseMode);
        actSkillMove = new ActSkillMove(baseMode);
    }
    public virtual void Update(float dt)
    {
        actHitBack.Update(dt);
        downGround.Update(dt);
        actSkillMove.Update(dt);
    }
}
public class DownGround
{
    public PlayerModel baseMode;
    public float gravityPercent = 1;
    public float grayvityAdd = 0f;
    public float startSpeed = 0f;
    private float jumpStartSpeed = 10;
    public float jumpCd = 0f;

    public bool isInGroud = false;
    public DownGround(PlayerModel mode)
    {
        baseMode = mode;
    }
    /// <summary>
    /// 上为负
    /// </summary>
    /// <param name="speed"></param>
    public void AddStartSpeed(float speed)
    {
        startSpeed -= speed;
    }
    public void StartJump()
    {
        Debugger.Log("开始跳跃" + jumpStartSpeed);
        AddStartSpeed(jumpStartSpeed);
    }
    private float GetGrayVity()
    {
        return Globals.gravity * gravityPercent + grayvityAdd;
    }
    public void Update(float dt)
    {
        if (jumpCd > 0)
        {
            jumpCd -= dt;
        }
        if (baseMode.posLocker.HasLocker())
        {
            //中了锁住位置的，初始速度为0
            startSpeed = 0f;
            return;
        }
        float lastSpeed = startSpeed;
        float offsetY = 0;
        if (!baseMode.Fly)
        {
            float gravity = GetGrayVity();
            startSpeed = startSpeed + gravity * dt;
            offsetY = (lastSpeed * lastSpeed - startSpeed * startSpeed) / (2 * gravity);
        }
        if (baseMode.controller && (baseMode.controller.isGrounded || TransformUtils.IsInGround(baseMode.transform)) && startSpeed >= 0)
        {
            isInGroud = true;
            startSpeed = 0f;
        }
        else
        {
            isInGroud = false;
        }

        Vector3 goStep = new Vector3(0, offsetY, 0);
        if (baseMode.Fly)
        {
            goStep = GroundPos(baseMode.transform.position);
        }
        baseMode.AddStep(goStep);
    }
    private Vector3 GroundPos(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(pos.x, pos.y + 0.5f, pos.z), Vector3.down, out hit, 1f,
            1 << Globals.GetDownLayer()))
        {
            return hit.point;
        }
        return new Vector3(10000,10001,10000);
    }
}
public class ActHitBack
{
    public PlayerModel baseMode;
    public delegate void ActHitEndAction(object param);
    protected ActHitEndAction _actHitEndAction = null;
    protected object _actParam;
    protected float hitbackCd = 0.5f;

    protected Vector3 _dest;
    protected Vector3 _actMovePosOri;//动作移动初始位置
    protected Vector3 _actMovePosDest;//动作移动目标位置
    protected Vector3 _actMoveDir = Vector3.zero;//动作移动向量
    protected Vector3 _lastDestPos = Vector3.zero;//上一个路径坐标
    protected float _actMoveDistance;//动作移动位移
    protected float _actMoveTime;//动作移动总共时间
    protected float _actMoveTimer;//动作移动剩下时间
    protected float _actPow;//动作移动平方
    public ActHitBack(PlayerModel mode)
    {
        baseMode = mode;
    }
    public Vector3 HitBack(Vector3 dir, float distance, float time, float actPow,ActHitEndAction action = null,object actParam = null)
    {
        baseMode.SetHitBackAnim(hitbackCd);
        _actHitEndAction = action;
        _actParam = actParam;
        return DoActmove(dir, distance, time, actPow, false);
    }
    protected Vector3 DoActmove(Vector3 dir, float distance, float time, float actPow, bool immediate)
    {
        Vector3 finDir = dir;
        finDir.y = 0;
        finDir = finDir.normalized;
        if (immediate)
        {
            Vector3 targetPos = baseMode.transform.position + finDir * distance;
            baseMode.transform.position = targetPos;
            if (_actHitEndAction != null)
            {
                _actHitEndAction(_actParam);
                _actHitEndAction = null;
            }
        }
        else
        {
            AddMoveLocker();
            _actMovePosOri = baseMode.transform.position;
            Vector3 targetPos = _actMovePosOri + finDir * distance;
            _actMovePosDest = targetPos;

            Vector3 diff = _actMovePosDest - _actMovePosOri;
            _actMoveDir = diff.normalized;
            _lastDestPos = _actMovePosOri;

            _actMoveDistance = diff.magnitude;
            _actMoveTime = time;
            _actMoveTimer = time;
            _actPow = actPow;
        }
        _dest = _actMovePosDest;
        return _actMovePosDest;
    }
    public virtual void AddMoveLocker()
    {
        baseMode.moveLocker.AddLocker("HitBack");
    }
    public virtual void RemoveLocker()
    {
        baseMode.moveLocker.RemoveLocker("HitBack");
    }
    public void Update(float dt)
    {
        if (_actMoveTimer > 0)
        {
            _actMoveTimer -= dt;
            if (_actMoveTimer <= 0)
            {
                Vector3 curDestPos = _actMoveDir * _actMoveDistance + _actMovePosOri;
                baseMode.MoveImmediate(curDestPos - _lastDestPos);

                _lastDestPos = curDestPos;
                ShutDownActMove();
            }
            else if (_actMoveTimer <= _actMoveTime)
            {
                float scale = (1 - _actMoveTimer / _actMoveTime);
                Vector3 curDestPos = _actMoveDir * Mathf.Pow(scale, _actPow) * _actMoveDistance + _actMovePosOri;
                baseMode.MoveImmediate(curDestPos - _lastDestPos);
                _lastDestPos = curDestPos;
            }
        }
    }
    public void ShutDownActMove()
    {
        _actMoveDistance = 0;
        _actMoveTime = 0;
        _actMoveTimer = 0;
        _actPow = 0;
        if (_actHitEndAction != null)
        {
            _actHitEndAction(_actParam);
            _actHitEndAction = null;
        }
        RemoveLocker();
    }
}
public class ActSkillMove : ActHitBack
{
    public ActSkillMove(PlayerModel model)
        : base(model)
    {

    }
    public Vector3 SetSkillMove(Vector3 dir, float distance, float time, float actPow, ActHitEndAction action = null, object actParam = null)
    {
        ShutDownActMove();
        baseMode.SetHitBackAnim(hitbackCd);
        _actHitEndAction = action;
        _actParam = actParam;
        return DoActmove(dir, distance, time, actPow, false);
    }
    public override void AddMoveLocker()
    {
    }
    public override void RemoveLocker()
    {
    }
}