using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BaseModel
{
    public CharacterController controller;
    public AnimCtrl animCtrl;
    public ActMoveCtrl actMoveCtrl;
    public override void InitBaseData(ModelInitData data,LoadedModelCallback callback = null)
    {
        base.InitBaseData(data, callback);
        baseSpeed = data.moveInfo.baseSpeed;
        baseSpeedPercent = data.moveInfo.baseSpeedPercent;
        addSpeed = data.moveInfo.addSpeed;
        addSpeedPercent = data.moveInfo.addSpeedPercent;
        maxAnimSpeed = data.moveInfo.maxAnimSpeed;
    }
    //初始化虚假的
    protected override void InitBaseCommponent(ModelData modelData)
    {
        base.InitBaseCommponent(modelData);
        RefreshCharaInfo();
        RefreshActMove();
    }
    protected override void OnStartModelInfo()
    {
        base.OnStartModelInfo();
        RefreshCharaInfo();
        RefreshAnimatorInfo();
        RefreshActMove();
    }

    protected override void SetColliderInfo(ModelInfo info)
    {
        GameObject collObj = ColliderUtil.SetGameObjectCollider(gameObject, info);
        SetCollider(collObj);
    }
    protected override void SetCharactorInfo(ModelInfo info)
    {
        CharacterControllerUtil.SetControllerData(gameObject, info);
    }
    protected virtual void RefreshCharaInfo()
    {
        controller = gameObject.GetComponent<CharacterController>();
        if (controller)
        {
            controller.enabled = true;
        }
    }
    protected virtual void RefreshAnimatorInfo()
    {
        if (animCtrl != null)
        {
            return;
        }
        animCtrl = new AnimatorCtrl();
        animCtrl.InitAnim(this);
    }
    protected virtual void PlayAnimByStateName(string name)
    {
        if (animCtrl != null)
        {
            animCtrl.PlayAnimByStateName(name);
        }
    }
    protected virtual void RefreshActMove()
    {
        if (actMoveCtrl != null)
        {
            return;
        }
        actMoveCtrl = new ActMoveCtrl();
        actMoveCtrl.Init(this);
    }

    #region 移动速度相关
    //速度相关
    private float baseSpeed = 2;
    private float baseSpeedPercent = 1;
    private float addSpeed = 0;
    private float addSpeedPercent = 0;
    protected float maxAnimSpeed;
    public void AddSpeedNum(float addValue)
    {
        addSpeed += addValue;
    }
    public void AddBasePercent(float addValue)
    {
        addSpeedPercent += addValue;
    }
    public void SetBasePercent(float value)
    {
        baseSpeedPercent = value;
    }
    private float GetFinalSpeed()
    {
        float finalSpeed = baseSpeed * (baseSpeedPercent);
        finalSpeed = finalSpeed < 0 ? 0 : finalSpeed;
        finalSpeed = (finalSpeed + addSpeed) * (1 + addSpeedPercent);
        return finalSpeed;
    }
    #endregion

    #region 寻路相关
    private static float sqrIgnoreOffsetLen = 0.5f * 0.5f;//接近目标的最小位置
    private float _radius;//设置移动偏移量
    private Vector3 _dest;//目标点
    public delegate void EndWayCallBack(object param = null);//移动到目标点回调
    protected EndWayCallBack _endCallBack;
    protected object _endCallParam;//回调参数
    public delegate void StartPathCallBack(Vector3[] path = null);//开始寻点下一次新的寻点回调
    protected StartPathCallBack _startPathCallBack;
    private Vector3[] _path;//点路径
    protected int _currentWaypoint;//当前路点
    private bool _indexPath;//开始寻点
    private int _index = 0;
    public bool moving = false;//是否处于寻路移动位移
    private float _forceTime = 0f;//锁住时间（锁住当前寻路）
    /// <summary>
    /// 寻路到地点
    /// </summary>
    public void GoToPosition(Vector3 dest,EndWayCallBack callback,object endCallParam,
        float forceTime = 0f,StartPathCallBack startPathCallBack = null)
    {
        _forceTime = forceTime;
        GoToPosition(dest, 0.0f, callback, endCallParam, startPathCallBack);
    }
    public void RadiusToPosition(Vector3 dest,float radius, EndWayCallBack callback, object endCallParam,
        StartPathCallBack startPathCallBack = null)
    {
        GoToPosition(dest, radius, callback, endCallParam, startPathCallBack);
    }
    private void GoToPosition(Vector3 dest, float radius, EndWayCallBack callback, object endCallParam = null,
            StartPathCallBack startPathCallBack = null)
    {
        if (playerEntity.IsDead)
        {
            return;
        }
        //与目的点极小的偏移不进行处理
        Vector3 destOffset = dest - _dest;
        if (destOffset.sqrMagnitude < sqrIgnoreOffsetLen)
        {
            //在移动中时才跳过极小的偏移修正
            if (moving)
            {
                return;
            }
        }

        moving = true;
        _radius = radius;
        _dest = dest;

        _endCallBack = callback;
        _endCallParam = endCallParam;

        _startPathCallBack = startPathCallBack;

        _path = new Vector3[2];
        _path[0] = transform.position;
        _path[1] = dest;

        if (!Fly && controller)
        {
            SeekPath(dest, callback, startPathCallBack);
        }
        else
        {
            DirectPath(dest, callback);
        }
    }

    /// <summary>
    /// 搜索路径
    /// </summary>
    /// <param name="des"></param>
    /// <param name="callback"></param>
    /// <param name="startPathBack"></param>
    public void SeekPath(Vector3 endPos, EndWayCallBack callback, StartPathCallBack startPathBack)
    {
        _index++;
        _indexPath = true;
        _endCallBack = callback;

        _startPathCallBack = startPathBack;
        AStarPathMgr.GetInstance().StartPath(transform.position, endPos, OnPathSeeked, _index);
    }
    private void OnPathSeeked(List<Vector3> p, int index)
    {
        if (index == _index && _indexPath)
        {
            _indexPath = false;
            Vector3[] ps = p.ToArray();
            SetPath(ps, 0);
            if (_startPathCallBack != null)
            {
                _startPathCallBack(ps);
                _startPathCallBack = null;
            }
        }
    }
    /// <summary>
    /// 根据路径去转向
    /// </summary>
    public void SetPath(Vector3[] path, int wayPointIndex)
    {
        _path = path;
        _currentWaypoint = wayPointIndex;
        if (path.Length > wayPointIndex)
        {
            SetDesRotation(path[wayPointIndex], false, TurnSpeed);
        }
    }
    public void DirectPath(Vector3 des, EndWayCallBack callback)
    {
        Vector3[] path = new Vector3[2];
        path[0] = transform.position;
        path[1] = des;
        SetPathDirect(path, 1, callback);
    }
    public void SetPathDirect(Vector3[] path, int wayPointIndex, EndWayCallBack callback)
    {
        if (path != null && path.Length == 0)
        {
            return;
        }
        _endCallBack = callback;
        SetPath(path, wayPointIndex);
    }

    private void NextMoveXunlu(float dt)
    {
        if (dt <= 0.0f)
        {
            return;
        }
        if (_radius > 0.0f && Vector3.Distance(transform.position + Vector3.down * transform.position.y,
            _dest + Vector3.down * _dest.y) < _radius)
        {
            ReachEndMove();
            return;
        }
        if (IsNotChangePos())
        {
            return;
        }
        bool needMoveNext = false;//是否移动下个路点
        Vector3 pa = _path[_currentWaypoint] - transform.position;
        Vector3 xunluVec = Vector3.zero;
        pa.y = 0;
        if (pa == Vector3.zero)
        {
            needMoveNext = true;
        }
        else
        {
            xunluVec = pa.normalized * GetFinalSpeed() * dt;
            Vector3 moveDir = pa.normalized;

            Vector3 ignorePa = pa;
            ignorePa.y = 0;
            if (moveDir == Vector3.zero || ignorePa.magnitude < 0.1f)//单位向量为0或者整个向量长度小于0.1f
            {
                needMoveNext = true;
            }
            else if (xunluVec.sqrMagnitude >= pa.sqrMagnitude)//跟据速度移动向量平方大于等于路径向量平方（等于路点）
            {
                transform.position = _path[_currentWaypoint];
                needMoveNext = true;
            }
        }
        if (!needMoveNext)
        {
            transform.Translate(xunluVec, Space.World);
        }
        else
        {
            _currentWaypoint++;
            if (_currentWaypoint < _path.Length)
            {
                SetDesRotation(_path[_currentWaypoint], false, TurnSpeed);
                float costTime = pa.magnitude / GetFinalSpeed();//算取路径所花时间
                dt -= costTime;
                NextMoveXunlu(dt);
            }
            else
            {
                ReachEndMove();
            }
        }
    }
    public virtual void ReachEndMove()
    {
        if (_endCallBack != null)
        {
            _endCallBack(_endCallParam);
            _endCallBack = null;
        }
        EndXunlun();
    }
    public void EndXunlun()
    {
        if (moving == false)
        {
            return;
        }
        moving = false;
        _path = null;
        _rotationRote = -1;
        _dest = transform.position;
        _endCallBack = null;
        _endCallParam = null;
        _indexPath = false;
        _currentWaypoint = 0;
    }
    #endregion

    #region 旋转角度相关
    //--旋转--角度相关
    private bool _lockDir;//锁定方向（每个寻路的锁住）
    private Quaternion _originRotation;//当前方向
    private Quaternion _targetRotation;//目标方向
    private float _rotationRote = -1;//每个dt时间旋转角度
    private float rotationSpeed = 6;//旋转速度
    public float TurnSpeed = 10f;//转向改变速度
    public bool needPathRotate = true;//需要随路径改变角度(总开关)

    /// <summary>
    /// 设置移动朝向
    /// dir表示的是是否相对向量
    /// </summary>
    public void SetDesRotation(Vector3 target, bool lockOneFrame, float speed, bool Dir = false)
    {
        if (!needPathRotate)
        {
            return;
        }
        if (_lockDir)
        {
            return;
        }
        if (speed == 0)
        {
            _rotationRote = -1;
            return;
        }
        _lockDir = lockOneFrame;
        _originRotation = transform.rotation;
        Vector3 dir;
        if (Dir)
        {
            dir = target;
        }
        else
        {
            dir = target - transform.position;
        }
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            _rotationRote = 0;
        }
        if (speed == -1)
        {
            transform.rotation = _targetRotation;
            _rotationRote = -1;
        }
        rotationSpeed = speed;
    }
    #endregion


    #region 手动移动相关
    private Vector3 _joyDir = Vector3.zero;//移动方向向量
    public bool joying = false;//是否处于手动移动位移
    public virtual void SetJoyDir(Vector3 value)
    {
        if (_forceTime > 0)
        {
            return;
        }
        if (_joyDir == value)
        {
            return;
        }
        if (_joyDir == Vector3.zero)
        {
            joying = true;
        }
        else if (value == Vector3.zero)
        {
            joying = false;
        }
        _joyDir = value;
    }
    public virtual Vector3 GetJoyDir()
    {
        return _joyDir;
    }
    private bool JoyMove(float dt)
    {
        if (GetJoyDir() == Vector3.zero)
        {
            return false;
        }
        EndXunlun();
        AddStep(GetJoyDir());
        SetDesRotation(GetJoyDir(), false, TurnSpeed, true);
        
        return true;
    }
    protected virtual void EndJoy(bool animTmm = false)
    {
        if (joying == false)
        {
            return;
        }
        joying = false;
        _rotationRote = -1;
        SetJoyDir(Vector3.zero);
    }
    #endregion

    #region 基础行动
    //被锁住
    protected Vector3 step = Vector3.zero;
    public void AddStep(Vector3 addStep)
    {
        step += addStep;
    }
    public Locker moveLocker = new Locker();
    public Locker posLocker = new Locker();
    public Locker rotationLocker = new Locker();
    public Locker animRunSpeedLocker = new Locker();
    public void AddRotationLocker(string name)
    {
        rotationLocker.AddLocker(name);
        _rotationRote = -1;
    }
    public bool IsNotChangePos()
    {
        return moveLocker.HasLocker() || posLocker.HasLocker();
    }
    public bool IsLockAnimRunSpeed()
    {
        return animRunSpeedLocker.HasLocker();
    }
    /// <summary>
    /// 强制移动，不放在stepMove里
    /// </summary>
    public void MoveImmediate(Vector3 offset)
    {
        if (!Fly)
        {
            //Debug.Log(offset);
            controller.Move(offset);
        }
        else
        {
            transform.Translate(offset, Space.World);
        }
    }

    public bool Fly;//表示是否可以穿越物体
    public float FlyHeight = 2f;
    public void SetFly(bool isValue, float value)
    {
        Fly = isValue;
        FlyHeight = value;
    }
    public virtual void SetHitBackAnim(float delayTime)
    {
        
    }

    public void StartStepMove(float dt)
    {
        float finalSpeed = GetFinalSpeed();
        float offsety = step.y;
        Vector3 goStep = step;
        goStep.y = 0;
        goStep = goStep * GetFinalSpeed() * dt;
        goStep.y = offsety;

        RefreshAnimSpeedFloat(finalSpeed);
        step = Vector3.zero;
        if (IsNotChangePos())
        {
            return;
        }
        if (goStep == Vector3.zero)
        {
            return;
        }
        if (!Fly && controller)
        {
            controller.Move(goStep);
        }
        else
        {
            transform.Translate(goStep, Space.World);
        }
    }

    protected virtual void RefreshAnimSpeedFloat(float finalSpeed)
    {
        
    }

    private void MoveUpdate(float dt)
    {
        //旋转步行
        if (_rotationRote != -1 && rotationLocker.HasLocker() == false)
        {
            _rotationRote += rotationSpeed * dt;
            if (_rotationRote >= 1)
            {
                transform.rotation = _targetRotation;
                _rotationRote = -1;
            }
            else
            {
                if (_originRotation != _targetRotation)
                {
                    transform.rotation = Quaternion.Lerp(_originRotation, _targetRotation, _rotationRote);
                }
                else
                {
                    _rotationRote = -1;
                }
            }
        }
        //解锁
        if (_forceTime > 0)
        {
            _forceTime = _forceTime - dt;
        }
        if (_forceTime <= 0 && JoyMove(dt))
        {
            return;
        }
        if (_path == null)
        {
            _forceTime = 0;
            return;
        }
        if (_path.Length <= _currentWaypoint)
        {
            ReachEndMove();
            return;
        }
        NextMoveXunlu(dt);
    }
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        Update(dt);
    }
    public virtual void Update(float dt)
    {
        actMoveCtrl.Update(dt);
        MoveUpdate(dt);
        StartStepMove(dt);
        if (animCtrl != null)
        {
            animCtrl.Update(dt);
        }
    }
    #endregion
}
