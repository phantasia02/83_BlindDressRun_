﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CMemoryShareBase
{
    public Vector3              m_OldPos;
    public float                m_TotleSpeed            = StaticGlobalDel.g_DefMovableTotleSpeed;
    public float[]              m_Buff                  = new float[(int)CMovableBase.ESpeedBuff.eMax];
    public int                  m_NumericalImage        = 0;
    public CMovableBase         m_MyMovable             = null;
    public Rigidbody            m_MyRigidbody           = null;
    public Transform            m_FloorRayStart         = null;
    public CMovableStateData[]  m_Data                  = new CMovableStateData[(int)StaticGlobalDel.EMovableState.eMax];
    public SplineFollower       m_MySplineFollower      = null;
    public int                  m_CurHpCount            = StaticGlobalDel.g_DefHp;
    public float                m_CurHpRatio            = StaticGlobalDel.g_DefHpRatio;
    public float                m_TargetHpRatio         = StaticGlobalDel.g_DefHpRatio;
    public Material             m_HpMat                 = null;
    public ParticleSystem[][]   m_AllFX                 = new ParticleSystem[(int)CGGameSceneData.EFXType.eMax][];

   // public GameObject           m_HpMat                 = null;
};

public class CMovableBase : CGameObjBas
{
    readonly int HpRatioID = Shader.PropertyToID("_Hp");

    public const float CRadius = 20.0f;

    public enum ESpeedBuff
    {
        eHit = 0,
        eMax
    };

    public enum EMovableType
    {
        eNull       = 0,
        ePlayer     = 1,
        eNpcAI      = 2,
        eMax
    };

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Handles.color = Color.red;
        //Vector3 pos = transform.position + Vector3.up * 1.0f;
        //Handles.DrawAAPolyLine(6, pos, pos + transform.forward * CMovableBase.CJumpObstacleDis);
        //Handles.color = Color.white;
    }
#endif


    public override EObjType ObjType() { return EObjType.eMovable; }
    //  public Rigidbody MyRigidbody { get { return m_MyRigidbody; } }


    protected Collider[] m_ChildCollider = null;
    protected GameObject m_ObjCollider = null;

    protected CAnimatorStateCtl m_AnimatorStateCtl = null;
    public CAnimatorStateCtl AnimatorStateCtl
    {
        set { m_AnimatorStateCtl = value; }
        get { return m_AnimatorStateCtl; }
    }

    protected CMovableStatePototype[] m_AllState = new CMovableStatePototype[(int)StaticGlobalDel.EMovableState.eMax];

    protected StaticGlobalDel.EMovableState m_CurState = StaticGlobalDel.EMovableState.eNull;
    public StaticGlobalDel.EMovableState CurState { get { return m_CurState; } }

    protected StaticGlobalDel.EMovableState m_OldState = StaticGlobalDel.EMovableState.eNull;
    public StaticGlobalDel.EMovableState OldState { get { return m_OldState; } }

    protected StaticGlobalDel.EMovableState m_ChangState = StaticGlobalDel.EMovableState.eMax;
    public StaticGlobalDel.EMovableState ChangState
    {
        set { m_ChangState = value; }
        get { return m_ChangState; }
    }

    // ==================== SerializeField ===========================================

    [SerializeField] protected EMovableType m_MyMovableType = EMovableType.eNull;
    public EMovableType MyMovableType { get { return m_MyMovableType; } }

    [SerializeField] protected Transform m_MyFloorStartPoint = null;
    public Transform MyFloorStartPoint { get { return m_MyFloorStartPoint; } }
    
    [SerializeField] protected MeshRenderer m_MyHpBarMesh   = null;
    [SerializeField] protected GameObject[] m_AllFXObj = null;

    // ==================== SerializeField ===========================================

    protected CMemoryShareBase m_MyMemoryShare = null;
    public CMemoryShareBase MyMemoryShare { get { return m_MyMemoryShare; } }

    public SplineFollower MySplineFollower { get { return m_MyMemoryShare.m_MySplineFollower; } }

    public int ImageNumber { get { return m_MyMemoryShare.m_NumericalImage; } }
    public float TotleSpeed { get { return m_MyMemoryShare.m_TotleSpeed; } }
    public float TotleSpeedRatio { get { return m_MyMemoryShare.m_TotleSpeed / StaticGlobalDel.g_DefMovableTotleSpeed; } }
    public int CurHpCount { get { return m_MyMemoryShare.m_CurHpCount; } }

    protected override void Awake()
    {
        m_ChildCollider = GetComponentsInChildren<Collider>();
        base.Awake();
    }

    

    protected virtual void CreateMemoryShare()
    {
        m_MyMemoryShare = new CMemoryShareBase();
        
        SetBaseMemoryShare();
    }

    protected void SetBaseMemoryShare()
    {
       

        m_MyMemoryShare.m_MyRigidbody           = this.GetComponent<Rigidbody>();
        m_MyMemoryShare.m_MySplineFollower      = this.GetComponent<SplineFollower>();
        m_MyMemoryShare.m_FloorRayStart         = m_MyFloorStartPoint;
        m_MyMemoryShare.m_HpMat                 = m_MyHpBarMesh.material;
        m_MyMemoryShare.m_MyMovable             = this;

        //ParticleSystem[] hdsjhdsbh = m_BeautifulObj.GetComponentsInChildren<ParticleSystem>();
        m_MyMemoryShare.m_AllFX[(int)CGGameSceneData.EFXType.eBeautiful]    = m_AllFXObj[(int)CGGameSceneData.EFXType.eBeautiful].GetComponentsInChildren<ParticleSystem>();
        m_MyMemoryShare.m_AllFX[(int)CGGameSceneData.EFXType.eUgly]         = m_AllFXObj[(int)CGGameSceneData.EFXType.eUgly].GetComponentsInChildren<ParticleSystem>();
        m_MyMemoryShare.m_AllFX[(int)CGGameSceneData.EFXType.eEnd]          = m_AllFXObj[(int)CGGameSceneData.EFXType.eEnd].GetComponentsInChildren<ParticleSystem>();

        //m_BeautifulObj.SetActive(false);
        //m_UglyObj.SetActive(false);

        //  m_MyMemoryShare.m_Data[(int)StaticGlobalDel.EMovableState.eCollision] = new CMovableCollisionBaseData();
    }

    protected virtual void AwakeEndSetNullState()
    {
        StaticGlobalDel.EMovableState lTempState = StaticGlobalDel.EMovableState.eNull;

        for (int i = 0; i < m_AllState.Length; i++)
        {
            lTempState = (StaticGlobalDel.EMovableState)i;

            if (lTempState == StaticGlobalDel.EMovableState.eNull || 
                m_AllState[i] != null)
                continue;

            switch (lTempState)
            {
                case StaticGlobalDel.EMovableState.eWait:
                    m_AllState[i] = new CWaitStateBase(this);
                    break;
                case StaticGlobalDel.EMovableState.eMove:
                    m_AllState[i] = new CMoveStateBase(this);
                    break;
                case StaticGlobalDel.EMovableState.eHit:
                    m_AllState[i] = new CHitStateBase(this);
                    break;
                case StaticGlobalDel.EMovableState.eWin:
                    m_AllState[i] = new CWinStateBase(this);
                    break;
                case StaticGlobalDel.EMovableState.eOver:
                    m_AllState[i] = new COverStateBase(this);
                    break;
            }
        }
    }

    protected void AwakeOK()
    {
        AwakeEndSetNullState();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
       // ShowEndFx(true);
    }

    public override void Init()
    {
        base.Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (m_MyMemoryShare.m_CurHpRatio != m_MyMemoryShare.m_TargetHpRatio)
        {
            m_MyMemoryShare.m_CurHpRatio = Mathf.Lerp(m_MyMemoryShare.m_CurHpRatio, m_MyMemoryShare.m_TargetHpRatio, 0.3f * Time.deltaTime);

            if (Mathf.Abs(m_MyMemoryShare.m_TargetHpRatio - m_MyMemoryShare.m_CurHpRatio) < 0.001f)
                m_MyMemoryShare.m_CurHpRatio = m_MyMemoryShare.m_TargetHpRatio;

            m_MyMemoryShare.m_HpMat.SetFloat(HpRatioID, m_MyMemoryShare.m_CurHpRatio);
        }

        if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        {
            m_AllState[(int)m_CurState].updataMovableState();
        }

        ChangStateFunc();

        m_MyMemoryShare.m_OldPos = transform.position;
    }

    public virtual void ChangStateFunc()
    {
        if (ChangState != StaticGlobalDel.EMovableState.eMax)
        {
            SetCurState(m_ChangState);
            
        }
    }

    public Vector3 PickRandomPoint()
    {
        var point = Random.insideUnitSphere * CRadius;

        point.y = 0;
        point += transform.position;
        return point;
    }


    protected virtual void LateUpdate()
    {
        
    }


    protected virtual void SetCurState(StaticGlobalDel.EMovableState pamState)
    {
        if (pamState == m_CurState )
            return;

        ChangState = StaticGlobalDel.EMovableState.eMax;
        StaticGlobalDel.EMovableState lTempOldState = m_CurState;

        if (lTempOldState != StaticGlobalDel.EMovableState.eNull)
        {
            if (m_AllState[(int)lTempOldState] != null)
                m_AllState[(int)lTempOldState].OutMovableState();
        }


        m_CurState = pamState;
        

        if (m_AllState[(int)m_CurState] != null)
             m_AllState[(int)m_CurState].InMovableState();
        
        m_OldState = lTempOldState;

        if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
            m_AllState[(int)m_CurState].updataMovableState();
    }

    public virtual void CheckOver()
    {
        //if (ChangState == StaticGlobalDel.EMovableState.eWait ||
        //    ChangState == StaticGlobalDel.EMovableState.eMove ||
        //    ChangState == StaticGlobalDel.EMovableState.eCollision ||
        //    CurState == StaticGlobalDel.EMovableState.eWait ||
        //    CurState == StaticGlobalDel.EMovableState.eMove ||
        //    CurState == StaticGlobalDel.EMovableState.eCollision)
        //{
        //    if (m_MyGameManager.GetFloorBouncingBedBoxCount(this.FloorNumber) == 0)
        //        ChangState = StaticGlobalDel.EMovableState.eOver;
        //}
    }

    public void DestroyThis()
    {
        StartCoroutine(StartCoroutineDestroyThis());
    }


    IEnumerator StartCoroutineDestroyThis()
    {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    public virtual void TouchBouncingBed(Collider other)
    {
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        //if (m_MyGameManager.CurState == CGameManager.EState.eNextWin || m_MyGameManager.CurState == CGameManager.EState.eWinUI)
        //    return;

 


        //if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        //    m_AllState[(int)m_CurState].OnTriggerEnter(other);
    }

    public virtual void OnTriggerStay(Collider other)
    {
        if (other.tag == "BouncingBed")
        {
            TouchBouncingBed(other);
        }

        if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
            m_AllState[(int)m_CurState].OnTriggerStay(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {

    }


    public virtual void OnCollisionEnter(Collision other)
    {
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
            m_AllState[(int)m_CurState].OnCollisionEnter(other);
    }

    public virtual void HitInput(RaycastHit hit)
    {

        if (m_AllState[(int)m_CurState] != null)
        {
            m_AllState[(int)m_CurState].Input(hit);
        }
    }

    public virtual void InputUpdata()
    {
        //if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        //{
        //    m_AllState[(int)m_CurState].Input();
        //}
    }

    public virtual void OnMouseDown()
    {
    }

    public virtual void OnMouseDrag()
    {
    }

    public virtual void OnMouseUp()
    {
    }

    public void OpenColliderFloor(bool lColliderFloor)
    {
        if (lColliderFloor)
        {
            for (int i = 0; i < m_ChildCollider.Length; i++)
                m_ChildCollider[i].gameObject.layer = (int)StaticGlobalDel.ELayerIndex.eMovable;


            m_MyMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            for (int i = 0; i < m_ChildCollider.Length; i++)
                m_ChildCollider[i].gameObject.layer = 0;

            m_MyMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }


    public void SetMoveBuff(ESpeedBuff type, float ratio)
    {
        m_MyMemoryShare.m_Buff[(int)type] = ratio;
        float lTempMoveRatio = 1.0f;

        for (int i = 0; i < m_MyMemoryShare.m_Buff.Length; i++)
            lTempMoveRatio *= m_MyMemoryShare.m_Buff[i];

        m_MyMemoryShare.m_TotleSpeed = StaticGlobalDel.g_DefMovableTotleSpeed * lTempMoveRatio;
        m_MyMemoryShare.m_MySplineFollower.followSpeed = m_MyMemoryShare.m_TotleSpeed;
    }

    public void ResetMoveBuff()
    {
        for (int i = 0; i < m_MyMemoryShare.m_Buff.Length; i++)
            m_MyMemoryShare.m_Buff[i] = 1.0f;

        m_MyMemoryShare.m_TotleSpeed = StaticGlobalDel.g_DefMovableTotleSpeed;
        m_MyMemoryShare.m_MySplineFollower.followSpeed = m_MyMemoryShare.m_TotleSpeed;
    }

    public void SetHpCount(int hpcount)
    {
        if (hpcount < 0)
            hpcount = 0;
        else if (hpcount >= StaticGlobalDel.g_MaxHp)
            hpcount = StaticGlobalDel.g_MaxHp;


        m_MyMemoryShare.m_CurHpCount = hpcount;

        if (m_MyMemoryShare.m_CurHpCount == StaticGlobalDel.g_MaxHp)
            m_MyMemoryShare.m_TargetHpRatio = 1.0f;
        else if (m_MyMemoryShare.m_CurHpCount == 0)
            m_MyMemoryShare.m_TargetHpRatio = 0.0f;
        else
            m_MyMemoryShare.m_TargetHpRatio = (float)m_MyMemoryShare.m_CurHpCount / (float)StaticGlobalDel.g_MaxHp;

        ParticleSystem[] lTempParticleSystem = null;
        if (m_MyMemoryShare.m_CurHpCount > StaticGlobalDel.g_RefFXGoodHp)
        {
            lTempParticleSystem = m_MyMemoryShare.m_AllFX[(int)CGGameSceneData.EFXType.eBeautiful];
            int lTempAddCount = m_MyMemoryShare.m_CurHpCount - StaticGlobalDel.g_RefFXGoodHp;

            for (int i = 0; i < lTempParticleSystem.Length; i++)
            {
                lTempParticleSystem[i].gameObject.SetActive(true);
                var lTempEmissionModule = lTempParticleSystem[i].emission;
                lTempEmissionModule.rateOverTime = 5.0f * (float)lTempAddCount * (float)lTempAddCount;
            }
        }
        else if (m_MyMemoryShare.m_CurHpCount < StaticGlobalDel.g_RefFXBadHp)
        {
            lTempParticleSystem = m_MyMemoryShare.m_AllFX[(int)CGGameSceneData.EFXType.eUgly];
            int lTempAddCount = m_MyMemoryShare.m_CurHpCount - StaticGlobalDel.g_RefFXBadHp;

            for (int i = 0; i < lTempParticleSystem.Length; i++)
            {
                lTempParticleSystem[i].gameObject.SetActive(true);
                var lTempEmissionModule = lTempParticleSystem[i].emission;
                lTempEmissionModule.rateOverTime = 5.0f * (float)lTempAddCount * (float)lTempAddCount;
            }
        }
        else
        {
            lTempParticleSystem = m_MyMemoryShare.m_AllFX[(int)CGGameSceneData.EFXType.eBeautiful];
            for (int i = 0; i < lTempParticleSystem.Length; i++)
                lTempParticleSystem[i].gameObject.SetActive(false);

            lTempParticleSystem = m_MyMemoryShare.m_AllFX[(int)CGGameSceneData.EFXType.eUgly];
            for (int i = 0; i < lTempParticleSystem.Length; i++)
                lTempParticleSystem[i].gameObject.SetActive(false);
        }

        Debug.Log($"m_MyMemoryShare.m_CurHpCount = { m_MyMemoryShare.m_CurHpCount }");

        if (m_MyMemoryShare.m_CurHpCount == 0)
            this.ChangState = StaticGlobalDel.EMovableState.eOver;
    }

    public void ShowEndFx(bool show)
    {
        ParticleSystem[] lTempParticleSystem = m_MyMemoryShare.m_AllFX[(int)CGGameSceneData.EFXType.eEnd];

        if (show)
        {
            CGGameSceneData.EFXEndMaterialType lTempFXEndMaterialType = CGGameSceneData.EFXEndMaterialType.eHappyGirl;

            if (m_MyMemoryShare.m_CurHpCount < StaticGlobalDel.g_RefFXBadHp)
                lTempFXEndMaterialType = CGGameSceneData.EFXEndMaterialType.eSadPeople;

            for (int i = 0; i < lTempParticleSystem.Length; i++)
            {
                lTempParticleSystem[i].gameObject.SetActive(show);
                var lTempRender = lTempParticleSystem[i].GetComponent<ParticleSystemRenderer>();
                lTempRender.material = Material.Instantiate(CGGameSceneData.SharedInstance.m_AllFXEndMaterial[(int)lTempFXEndMaterialType]);
            }
        }
        else
        {
            for (int i = 0; i < lTempParticleSystem.Length; i++)
                lTempParticleSystem[i].gameObject.SetActive(show);
        }
    }
}
