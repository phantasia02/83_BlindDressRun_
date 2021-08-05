using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CMemoryShareBase
{
    public Vector3              m_OldPos;
    public float                m_TotleSpeed            = 5.0f;
    public int                  m_NumericalImage        = 0;
    public CMovableBase         m_MyMovable             = null;
    public Rigidbody            m_MyRigidbody           = null;
    public Transform            m_FloorRayStart         = null;
    public CMovableStateData[]  m_Data                  = new CMovableStateData[(int)StaticGlobalDel.EMovableState.eMax];
};

public class CMovableBase : CGameObjBas
{
    public const float CRadius = 20.0f;

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
    [SerializeField] protected EMovableType m_MyMovableType = EMovableType.eNull;
    public EMovableType MyMovableType { get { return m_MyMovableType; } }

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

    [SerializeField] protected Transform m_MyFloorStartPoint = null;
    public Transform MyFloorStartPoint { get { return m_MyFloorStartPoint; } }

    protected CMemoryShareBase m_MyMemoryShare = null;
    public CMemoryShareBase MyMemoryShare { get { return m_MyMemoryShare; } }

    public int ImageNumber { get { return m_MyMemoryShare.m_NumericalImage; } }


    protected override void Awake()
    {
        m_ChildCollider = GetComponentsInChildren<Collider>();

       // for (int i = 0; i )

        base.Awake();
        AwakeOK();
    }

    protected virtual void CreateMemoryShare()
    {
        m_MyMemoryShare = new CMemoryShareBase();
        
        SetBaseMemoryShare();
    }

    protected void SetBaseMemoryShare()
    {
        m_MyMemoryShare.m_MyRigidbody   = this.GetComponent<Rigidbody>();
        m_MyMemoryShare.m_FloorRayStart = m_MyFloorStartPoint;
        
        m_MyMemoryShare.m_MyMovable = this;

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
                  //  m_AllState[i] = new CWaitStateBase(this);
                    break;
                case StaticGlobalDel.EMovableState.eMove:
                    break;
            }
        }

        //if (m_AllState[(int)StaticGlobalDel.EMovableState.eWait] == null)
        //    m_AllState[(int)StaticGlobalDel.EMovableState.eWait] = new CWaitStateBase(this);

        //if (m_AllState[(int)StaticGlobalDel.EMovableState.eMove] == null)
        //    m_AllState[(int)StaticGlobalDel.EMovableState.eMove] = new CMoveStateBase(this);

        //if (m_AllState[(int)StaticGlobalDel.EMovableState.eJump] == null)
        //    m_AllState[(int)StaticGlobalDel.EMovableState.eJump] = new CJumpStateBase(this);

        //if (m_AllState[(int)StaticGlobalDel.EMovableState.eCollision] == null)
        //    m_AllState[(int)StaticGlobalDel.EMovableState.eCollision] = new CMovableCollisionBase(this);

        //if (m_AllState[(int)StaticGlobalDel.EMovableState.eWin] == null)
        //    m_AllState[(int)StaticGlobalDel.EMovableState.eWin] = new CWinStateBase(this);

        //if (m_AllState[(int)StaticGlobalDel.EMovableState.eOver] == null)
        //    m_AllState[(int)StaticGlobalDel.EMovableState.eOver] = new COverStateBase(this);
    }

    protected void AwakeOK()
    {
        AwakeEndSetNullState();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        //CAnimatorStateCtl lTempAnimatorStateCtl = AnimatorStateCtl;
        //if (lTempAnimatorStateCtl)
        //    lTempAnimatorStateCtl.AnimatorSpeed = 0.0f;

        // GameObject lTempObj = CGGameSceneData.SharedInstance.m_AllDynamicallyCreateObj[(int)CGGameSceneData.EDynamicallyCreateObj.eFloor];
    }

    public override void Init()
    {

        
        base.Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
}
