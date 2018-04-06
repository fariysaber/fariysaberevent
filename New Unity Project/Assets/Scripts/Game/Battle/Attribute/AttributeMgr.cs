using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeMgr
{
    public PlayerEntity entity;
    public GamePlayerEntityData gameEntitydata;
    public PlayerWorldUI playerWorldUI;
    public float energyAddTime;
    public float hpAddTime;
    private int hp;
    public int HP
    {
        set {
            if (value < 0)
            {
                hp = 0;
            }
            else
            {
                hp = value;
                int maxHp = GetFinalHp();
                if (hp > maxHp)
                {
                    hp = maxHp;
                }
                playerWorldUI.SetHp(hp, maxHp);
            }
        }
        get { return hp; }
    }
    public void AddHp(int value,bool baoji = false)
    {
        HP += value;
        playerWorldUI.SetHpChange(value, entity.model.GetHeight(), baoji);
    }
    public bool IsDiyuPercentHp(float percent)
    {
        int maxHp = GetFinalHp();
        if (HP < maxHp * percent)
        {
            return true;
        }
        return false;
    }
    public int energy;
    public int Energy
    {
        set
        {
            if (value < 0)
            {
                energy = 0;
            }
            else
            {
                energy = value;
                int maxenergy = GetFinalEnergy();
                if (energy > maxenergy)
                {
                    energy = maxenergy;
                }
                playerWorldUI.SetEnergy(energy, maxenergy);
            }
        }
        get { return energy; }
    }
    public void AddEnergy(int value,bool isDebug = true)
    {
        Energy += value;
        if (isDebug)
            Debugger.Log("剩余能量" + Energy,Color.red);
    }
    public AttributeMgr()
    {
        
    }
    public void InitAttri(EntityInitData data, PlayerEntity entity)
    {
        this.entity = entity;
        gameEntitydata = data.gameEntityData as GamePlayerEntityData;
        AddEquipBuff();
        InitWorldUI();
        ResetInfo();
    }
    private void AddEquipBuff()
    {
        PlayerEquipInfo wuqi = gameEntitydata.wuqi;
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (wuqi != null)
        {
            for (int i = 0; i < wuqi.vo.buffid.Count; i++)
            {
                batentity.buffMgr.AddBuff(wuqi.vo.buffid[i]);
            }
        }
        List<int> buffs = gameEntitydata.GetLingzhuangBuff();
        for (int i = 0; i < buffs.Count; i++)
        {
            batentity.buffMgr.AddBuff(buffs[i]);
        }
    }
    public GameObject wuqiObject;
    public void StartLoadEquip()
    {
        if (wuqiObject != null)
        {
            GameObject.DestroyImmediate(wuqiObject);
        }
        if (gameEntitydata.wuqi == null || gameEntitydata.wuqi.vo.model.Equals(""))
        {
            return;
        }
        ResourceMgr.GetInstance().LoadResource(ResourceType.entity,ResourcePath.GetEquip(gameEntitydata.wuqi.vo.model), OnEquipFinishLoad);
    }
    public void OnEquipFinishLoad(ResourceData data)
    {
        if (entity.IsDispose)
        {
            return;
        }
        wuqiObject = data.GetCreateObject();
        entity.model.AttachEffectObject(wuqiObject, Vector3.zero, Vector3.zero, "wuqinode");
    }
    private void ResetInfo()
    {
        int maxHP = GetFinalHp();
        HP = maxHP;

        int maxEnergy = GetFinalEnergy();
        Energy = maxEnergy;

        playerWorldUI.SetHp(HP, GetFinalHp());
        playerWorldUI.SetEnergy(Energy, GetFinalEnergy());
    }
    private void InitWorldUI()
    {
        if (playerWorldUI != null)
        {
            playerWorldUI.DestroyObj();
            playerWorldUI = null;
        }
        Transform parent = entity.model.GetAttributeObj();
        playerWorldUI = new PlayerWorldUI(parent.gameObject, null, "sliderview");
        playerWorldUI.LoadResource();
        playerWorldUI.SetHpBar(Globals.GetHpBarColor(entity.EntityType));
    }


    //获取属性和设置属性的外部接口
    public int GetFinalAtk()
    {
        int attriAtk = gameEntitydata.GetAtk();
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (batentity.buffMgr != null)
        {
            attriAtk += batentity.buffMgr.GetAtkBuffNum();
            attriAtk = attriAtk * (1000 + batentity.buffMgr.GetAtkAddPercent()) / 1000;
        }
        return attriAtk;
    }
    public int GetFinalDef()
    {
        int attriDef = gameEntitydata.GetDefend();
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (batentity.buffMgr != null)
        {
            attriDef += batentity.buffMgr.GetDefBuffNum();
            attriDef = attriDef * (1000 + batentity.buffMgr.GetDefAddpercent()) / 1000;
        }
        return attriDef;
    }
    public int GetFinalHp()
    {
        int attriHp = gameEntitydata.GetHp();
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (batentity.buffMgr != null)
        {
            attriHp += batentity.buffMgr.GetHpBuffNum();
            attriHp = attriHp * (1000 + batentity.buffMgr.GetHpAddpercent()) / 1000;
        }
        return attriHp;
    }
    public float GetFinalAddHpCd()
    {
        return -1;
    }
    public int GetFinalEnergy()
    {
        return 100;
    }
    public float GetFinalAddEnergyCd()
    {
        return 0.5f;
    }
    public float GetFinalBaoji()
    {
        float attribaoji = gameEntitydata.GetBao();
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (batentity.buffMgr != null)
        {
            attribaoji += batentity.buffMgr.GetBaojiBuffNum();
            attribaoji = attribaoji * (1000 + batentity.buffMgr.GetBaojiAddpercent()) / 1000f;
        }
        return attribaoji;
    }
    public float GetFinalBaoShang()
    {
        float attribaoshang = gameEntitydata.GetBaoshang();
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (batentity.buffMgr != null)
        {
            attribaoshang += batentity.buffMgr.GetBaoShangBuffNum();
            attribaoshang = attribaoshang * (1000 + batentity.buffMgr.GetBaoShangAddpercent()) / 1000f;
        }
        return attribaoshang;
    }
    public float GetFinalAddDmg()
    {
        float attradddmg = 0;
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (batentity.buffMgr != null)
        {
            attradddmg += batentity.buffMgr.GetAddDmg() / 1000f;
        }
        return attradddmg;
    }
    public float GetFinalReduDmg()
    {
        float attrredudmg = 0;
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (batentity.buffMgr != null)
        {
            attrredudmg += batentity.buffMgr.GetReduDmg() / 1000f;
        }
        return attrredudmg;
    }
    public float GetChuantou()
    {
        float attrrechuantou = 0;
        BattlePlayerEntity batentity = entity as BattlePlayerEntity;
        if (batentity.buffMgr != null)
        {
            attrrechuantou += batentity.buffMgr.GetChuantou() / 1000f;
        }
        return attrrechuantou;
    }
    public int GetMaxEnergy()
    {
        return 100;
    }
    public virtual void SetShanghai(Bullet bullet, float finalShanghai, bool baoji)
    {

    }
    public virtual void OnUpdate(float dt)
    {
        energyAddTime += dt;
        float finalAddEnergeCd = GetFinalAddEnergyCd();
        while (energyAddTime > finalAddEnergeCd && finalAddEnergeCd > 0)
        {
            energyAddTime -= finalAddEnergeCd;
            AddEnergy(1, false);
        }
        hpAddTime += dt;
        float finalAddHpCd = GetFinalAddHpCd();
        while (hpAddTime > finalAddHpCd && finalAddHpCd > 0)
        {
            hpAddTime -= finalAddHpCd;
            AddHp(1);
        }
        UpdateWord(dt);
    }
    public virtual void UpdateWord(float dt)
    {
        playerWorldUI.Update(dt);
    }
    public virtual void DestroyAll()
    {
        if (playerWorldUI != null)
            playerWorldUI.DestroyObj();
    }
}


public class PlayerWorldUI : ChildPanel
{
    private Transform HpSlider;
    private UIImage hpbackImg;
    private UIImage hpFill;
    private Slider hpSliders;

    private Transform EnergySlider;
    private UIImage energybackImg;
    private UIImage energyFill;
    private Slider energySliders;

    private GameObject rootobj;
    private string hppic = "";
    private int hp = 1;
    private int maxhp = 1;
    private int energy = 1;
    private int maxenergy = 1;

    private List<AttriNumber> attriNumber = new List<AttriNumber>();
    private GameObject attrnumberParent;
    public PlayerWorldUI(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void LoadResCallBack()
    {
        base.LoadResCallBack();
        rootobj = root.transform.Find("scale").gameObject;
        TransformUtils.ResetTransform(root.gameObject);
        foreach (Transform child in root.transform.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = Globals.playerlayer;
        }
        Transform sliderBar = rootobj.transform.Find("Slider");
        TransformUtils.SetVisible(sliderBar.gameObject, false);

        HpSlider = TransformUtils.CreateGameObject(sliderBar, rootobj.transform, new Vector3(0, 11f, 0), Vector3.one, "HpSlider").transform;
        hpSliders = HpSlider.GetComponent<Slider>();
        EnergySlider = TransformUtils.CreateGameObject(sliderBar, rootobj.transform, new Vector3(0, 5f, 0), Vector3.one, "EnergySlider").transform;
        energySliders = EnergySlider.GetComponent<Slider>();

        hpbackImg = new UIImage(HpSlider, "Background");
        hpFill = new UIImage(HpSlider.Find("Fill Area"), "Fill");
        UIMgr.GetInstance().GetSprite(hpbackImg._image, ResourcePath.battlesprote, "pic_blackbar");
        TransformUtils.SetVisible(HpSlider.gameObject, true);

        energybackImg = new UIImage(EnergySlider, "Background");
        energyFill = new UIImage(EnergySlider.Find("Fill Area"), "Fill");
        UIMgr.GetInstance().GetSprite(energybackImg._image, ResourcePath.battlesprote, "pic_blackbar");
        UIMgr.GetInstance().GetSprite(energyFill._image, ResourcePath.battlesprote, "pic_zibar");
        TransformUtils.SetVisible(EnergySlider.gameObject, true);

        if (!hppic.Equals(""))
        {
            SetHpBar(hppic);
        }
        SetHp(hp, maxhp);
        SetEnergy(hp, maxenergy);

        attrnumberParent = new GameObject("attrinumberParent");
        attrnumberParent.transform.SetParent(rootobj.transform);
        TransformUtils.ResetTransform(attrnumberParent);
    }
    public void Update(float dt)
    {
        for (int i = attriNumber.Count - 1; i >= 0; i--)
        {
            attriNumber[i].Update(dt);
        }
    }
    public void SetHpBar(string pic)
    {
        hppic = pic;
        if (hpFill != null)
        {
            UIMgr.GetInstance().GetSprite(hpFill._image, ResourcePath.battlesprote, pic);
        }
    }
    public void SetHp(int getvalue,int maxvalue)
    {
        hp = getvalue;
        maxhp = maxvalue;
        if (hpSliders != null)
            hpSliders.value = (float)hp / (float)maxhp * 100;
    }
    public void SetHpChange(int value,float height,bool baoji = false)
    {
        if (attrnumberParent == null)
        {
            return;
        }

        AttriNumber attrinum = new AttriNumber(value, baoji, attrnumberParent, height / rootobj.transform.localScale.x);
        AttriNumber.EndCallBack end = delegate()
        {
            attriNumber.Remove(attrinum);
        };
        attrinum.endcallback = end;
        attriNumber.Add(attrinum);
    }
    public void SetEnergy(int getvalue, int maxvalue)
    {
        energy = getvalue;
        maxenergy = maxvalue;
        if (energySliders != null)
            energySliders.value = (float)energy / (float)maxenergy * 100;
    }
    public virtual void DestroyObj()
    {
        for (int i = attriNumber.Count - 1; i >= 0; i--)
        {
            attriNumber[i].Destroy();
        }
        if(root != null)
        {
            GameObject.DestroyImmediate(root.gameObject);
            base.Destroy();
        }
    }
}

public class AttriNumber
{
    public GameObject numParent;
    public List<GameObject> numsObject;
    public List<Image> numsImg;
    public delegate void EndCallBack();
    public EndCallBack endcallback;
    public float flyTime;
    public float maxFlyTime = 1.5f;
    public float firstStatetime = 1f;
    public float posBtween = 6f;
    public float xSpeed;
    public float yAttr;
    public float ySpeed;
    public AttriNumType type = AttriNumType.normalreduce;
    public bool IsDestroy;
    public AttriNumber(int blood,bool baoji,GameObject parent,float height)
    {
        numsObject = new List<GameObject>();
        numsImg = new List<Image>();
        int num = blood;
        if (num > 0)
        {
            type = AttriNumType.normaladd;
        }
        else
        {
            if(baoji)
            {
                type = AttriNumType.baojireduce;
            }
            else
            {
                type = AttriNumType.normalreduce;
            }
        }
        int absNum = Mathf.Abs(num);
        while (absNum > 0)
        {
            int keyNum = absNum % 10;
            GameObject crt = SceneMgr.GetInstance().nowGameScene.GetNumsObject(keyNum);
            if (crt == null)
            {
                crt = new GameObject();
                Image img = crt.AddComponent<Image>();
                UIMgr.GetInstance().GetSprite(img, ResourcePath.numsprite, "pic_num" + keyNum);
                crt.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
                crt.name = "" + keyNum;
            }
            numsObject.Add(crt);
            numsImg.Add(crt.GetComponent<Image>());
            absNum /= 10;
        }
        numParent = SceneMgr.GetInstance().nowGameScene.GetNumsObject(-1);
        if (numParent == null)
        {
            numParent = new GameObject("numparent");
        }
        for (int i = 0; i < numsObject.Count; i++)
        {
            TransformUtils.SetParent(numsObject[i].transform, numParent.transform, true);
            Image img = numsObject[i].GetComponent<Image>();
            if (type == AttriNumType.normalreduce)
            {
                img.color = Color.white;
                xSpeed = UnityEngine.Random.Range(-100f,100f) >= 0 ? 20f : -20f;
                ySpeed = 30f;
                //maxFlyTime = 1f;
                yAttr = 40f;
            }
            else if (type == AttriNumType.baojireduce)
            {
                img.color = Color.yellow;
                numsObject[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                posBtween = 8f;
                //maxFlyTime = 1.5f;
            }
            else if (type == AttriNumType.normaladd)
            {
                img.color = Color.green;
                ySpeed = 20f;
                maxFlyTime = 1f;
                firstStatetime = 0.5f;
            }
            numsObject[i].transform.localPosition = new Vector3(GetPosX(i + 1, numsObject.Count, posBtween), 0, 0);
        }
        TransformUtils.SetParent(numParent.transform, parent.transform, true);
        numParent.transform.localPosition = new Vector3(UnityEngine.Random.Range(-3f,3f),
            -height / 4 + UnityEngine.Random.Range(-5f, 5f), 0);
        if (type == AttriNumType.baojireduce)
        {
            Vector3 getPos = numParent.transform.localPosition;
            getPos.y = -height / 8;
            numParent.transform.localPosition = getPos;
        }
        flyTime = maxFlyTime;
    }
    public void Update(float dt)
    {
        if (IsDestroy)
        {
            return;
        }
        if (flyTime > 0)
        {
            flyTime -= dt;
            if (flyTime <= 0)
            {
                if (endcallback != null)
                    endcallback();
                Destroy();
            }
            else
            {
                Vector3 nowPos = numParent.transform.localPosition;
                ySpeed -= yAttr * dt;
                nowPos.x += xSpeed * dt;
                nowPos.y += ySpeed * dt;
                numParent.transform.localPosition = nowPos;
                if (flyTime <= firstStatetime)
                {
                    for (int i = 0; i < numsImg.Count; i++)
                    {
                        Color color = numsImg[i].color;
                        numsImg[i].color = new Color(color.r, color.g, color.b, flyTime / (maxFlyTime - firstStatetime));
                    }
                }
            }
        }
    }
    public void Destroy()
    {
        if (IsDestroy)
        {
            return;
        }
        IsDestroy = true;
        for(int i = 0;i<numsObject.Count;i++)
        {
            int key = 0;
            int.TryParse(numsObject[i].name,out key);
            SceneMgr.GetInstance().nowGameScene.AddNumsObject(key, numsObject[i]);
        }
        SceneMgr.GetInstance().nowGameScene.AddNumsObject(-1, numParent);
    }
    public static float GetPosX(int index,int maxIndex, float width)
    {
        float center = (float)(maxIndex + 1) / 2f;
        float posx = width * (center - index);
        return posx;
    }
}
public enum AttriNumType
{
    normalreduce = 0,
    baojireduce = 1,
    normaladd = 2,
}