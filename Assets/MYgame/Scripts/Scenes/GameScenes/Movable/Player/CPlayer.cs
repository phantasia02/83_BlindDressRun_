using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CPlayerMemoryShare : CMemoryShareBase
{
    public bool         m_bDown                             = false;
    public Vector3      m_OldMouseDownPos                   = Vector3.zero;
    public CinemachineVirtualCamera m_PlayerNormalCamera    = null;
    public CinemachineVirtualCamera m_PlayerJumpCamera      = null;
    public CinemachineFreeLook m_PlayerWinCamera            = null;
    public CinemachineImpulseSource m_CameraShock           = null;
    public GameObject   m_FogPlane                          = null;
    public GameObject   m_TouchBouncingBed                  = null;
    public Vector3      m_OldMouseDragDirNormal             = Vector3.zero;
};

public class CPlayer : CMovableBase
{
    protected float m_MaxMoveDirSize = 0;

    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    [SerializeField] CinemachineVirtualCamera m_PlayerNormalCamera;
    public CinemachineVirtualCamera PlayerNormalFollowObj { get { return m_PlayerNormalCamera; } }

    [SerializeField] CinemachineVirtualCamera m_PlayerJumpCamera;
    public CinemachineVirtualCamera PlayerJumpFollowObj { get { return m_PlayerJumpCamera; } }

    [SerializeField] CinemachineFreeLook m_PlayerWinCamera;
    [SerializeField] GameObject m_FogPlane;

    public Vector3 m_OldMouseDragDir = Vector3.zero;

    protected override void Awake()
    {
        CreateMemoryShare();

        //m_AllState[(int)StaticGlobalDel.EMovableState.eWait] = new CWaitStatePlayer(this);
        //m_AllState[(int)StaticGlobalDel.EMovableState.eMove] = new CMoveStatePlayer(this);
        //m_AllState[(int)StaticGlobalDel.EMovableState.eJump] = new CJumpStatePlayer(this);
        //m_AllState[(int)StaticGlobalDel.EMovableState.eWin]  = new CWinStatePlayer(this);
        //m_AllState[(int)StaticGlobalDel.EMovableState.eOver]  = new COverStatePlayer(this);

        base.Awake();

        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 10.0f;
    }


    protected override void CreateMemoryShare()
    {
        m_MyPlayerMemoryShare = new CPlayerMemoryShare();
        m_MyMemoryShare = m_MyPlayerMemoryShare;

        m_MyPlayerMemoryShare.m_PlayerNormalCamera  = m_PlayerNormalCamera;
        m_MyPlayerMemoryShare.m_PlayerJumpCamera    = m_PlayerJumpCamera;
        m_MyPlayerMemoryShare.m_PlayerWinCamera     = m_PlayerWinCamera;
        m_MyPlayerMemoryShare.m_CameraShock         = this.GetComponent<CinemachineImpulseSource>();
        m_MyPlayerMemoryShare.m_FogPlane            = m_FogPlane;

        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
       // SetCurState(StaticGlobalDel.EMovableState.eWait);


        m_MyMemoryShare.m_FloorNumber = (int)((this.transform.position.y + 1.0f) / 30.0f);



    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady || m_MyGameManager.CurState == CGameManager.EState.eReadyWin)
            InputUpdata();

        //CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
        //if (lTempGameSceneWindow && lTempGameSceneWindow.GetShow())
        //    lTempGameSceneWindow.SetBouncingBedCount(m_MyGameManager.GetFloorBouncingBedBoxCount(m_MyMemoryShare.m_FloorNumber));
    }

    protected override void LateUpdate()
    {
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        {
            m_AllState[(int)m_CurState].LateUpdate();
        }
    }

    public override void InputUpdata()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            PlayerMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            PlayerMouseUp();
        }
    }


    //public override void OnMouseDown()
    //{
    //}

    //public override void OnMouseDrag()
    //{
    //}

    //public override void OnMouseUp()
    //{
    //}

    public bool PlayerCtrl()
    {
        return true;
        //return !(CurState == StaticGlobalDel.EMovableState.eJump || CurState == StaticGlobalDel.EMovableState.eCollision || CurState == StaticGlobalDel.EMovableState.eWin);
    }

    public void PlayerMouseDown()
    {
        if (!PlayerCtrl())
        {
            if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
            {
                m_AllState[(int)m_CurState].MouseDown();
            }
        }

        m_MyPlayerMemoryShare.m_bDown = true;
        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
        m_MyPlayerMemoryShare.m_OldMouseDragDirNormal = this.transform.forward;
        m_MyPlayerMemoryShare.m_OldMouseDragDirNormal.y = 0.0f;
        m_OldMouseDragDir = m_MyPlayerMemoryShare.m_OldMouseDragDirNormal;
    }

    public void PlayerMouseDrag()
    {
        if (!PlayerCtrl())
            return;

        if (!m_MyPlayerMemoryShare.m_bDown)
            return;

    

        Vector3 lTempMouseDrag = Input.mousePosition - m_MyPlayerMemoryShare.m_OldMouseDownPos;
        lTempMouseDrag.z = lTempMouseDrag.y;
        lTempMouseDrag.y = 0.0f;

       // ChangState = StaticGlobalDel.EMovableState.eMove;
        m_OldMouseDragDir += lTempMouseDrag;
        m_OldMouseDragDir.y = 0.0f;

        m_OldMouseDragDir = Vector3.ClampMagnitude(m_OldMouseDragDir, m_MaxMoveDirSize);
        m_MyPlayerMemoryShare.m_OldMouseDragDirNormal = m_OldMouseDragDir;
        m_MyPlayerMemoryShare.m_OldMouseDragDirNormal.Normalize();
        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;

    }

    public void PlayerMouseUp()
    {
        //if (m_MyPlayerMemoryShare.m_bDown)
        //{
        //    m_MyPlayerMemoryShare.m_bDown = false;
        //    m_MyPlayerMemoryShare.m_OldMouseDownPos = Vector3.zero;

        //    if (CurState == StaticGlobalDel.EMovableState.eMove)
        //        ChangState = StaticGlobalDel.EMovableState.eWait;
        //}
    }

    public override void TouchBouncingBed(Collider other)
    {
        //if (other.tag == "BouncingBed" &&
        //    (CurState == StaticGlobalDel.EMovableState.eWait))
        //{
        //    m_MyPlayerMemoryShare.m_TouchBouncingBed = other.gameObject;
        //    base.TouchBouncingBed(other);
        //}
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag == "Fuel")
        {
            CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
            if (lTempGameSceneWindow)
                lTempGameSceneWindow.SetFuelNumber(Fuel);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.tag == "BouncingBed")
            m_MyPlayerMemoryShare.m_TouchBouncingBed = null;
    }
}
