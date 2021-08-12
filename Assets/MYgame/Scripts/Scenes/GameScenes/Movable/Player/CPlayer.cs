﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Dreamteck.Splines;

public class CPlayerMemoryShare : CMemoryShareBase
{
    public bool                     m_bDown                 = false;
    public Vector3                  m_OldMouseDownPos       = Vector3.zero;
    public CinemachineVirtualCamera m_PlayerNormalCamera    = null;
    public CinemachineVirtualCamera m_PlayerWinLoseCamera   = null;
    public GameObject               m_TouchBouncingBed      = null;
    public Vector3                  m_OldMouseDragDirNormal = Vector3.zero;
    public SplineFollower           m_DamiCameraFollwer     = null;
    public CPlayer                  m_MyPlayer              = null;
    public GameObject[]             m_AllHpBarObj           = null;
};

public class CPlayer : CMovableBase
{


    protected float m_MaxMoveDirSize = 0;

    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    [SerializeField] CinemachineVirtualCamera m_PlayerNormalCamera;
    public CinemachineVirtualCamera PlayerNormalFollowObj { get { return m_PlayerNormalCamera; } }

    [SerializeField] CinemachineVirtualCamera m_PlayerWinLoseCamera;
    public CinemachineVirtualCamera PlayerWinLoseCamera { get { return m_PlayerWinLoseCamera; } }

    [SerializeField] CRoleAccessories[] m_AllReplaceableAccessories;

    [SerializeField] GameObject[] m_AllHpBarObj;

    protected Vector3 m_OldMouseDragDir = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        CreateMemoryShare();

        m_AllState[(int)StaticGlobalDel.EMovableState.eWait]    = new CWaitStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove]    = new CMoveStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eHit]     = new CHitStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eWin]     = new CWinStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eOver]    = new COverStatePlayer(this);

        

        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 10.0f;


        const int CDefMatIndex = 1;
        for (int i = 0; i < m_AllReplaceableAccessories.Length; i++)
            m_AllReplaceableAccessories[i].SetUpdateMat(CDefMatIndex);

        AwakeOK();
    }


    protected override void CreateMemoryShare()
    {
        m_MyPlayerMemoryShare = new CPlayerMemoryShare();
        m_MyMemoryShare = m_MyPlayerMemoryShare;

        m_MyPlayerMemoryShare.m_PlayerNormalCamera  = m_PlayerNormalCamera;
        m_MyPlayerMemoryShare.m_PlayerWinLoseCamera = m_PlayerWinLoseCamera;
        //m_MyPlayerMemoryShare.m_PlayerWinCamera     = m_PlayerWinCamera;
        //m_MyPlayerMemoryShare.m_CameraShock         = this.GetComponent<CinemachineImpulseSource>();
        //m_MyPlayerMemoryShare.m_MySplineFollower    = this.GetComponent<SplineFollower>();
        m_MyPlayerMemoryShare.m_DamiCameraFollwer   = m_MyGameManager.DamiCameraFollwer.GetComponent<SplineFollower>();
        m_MyPlayerMemoryShare.m_MyPlayer            = this;
        m_MyPlayerMemoryShare.m_AllHpBarObj         = m_AllHpBarObj;

        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetCurState(StaticGlobalDel.EMovableState.eWait);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        

        if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady)
            InputUpdata();


       // m_MyPlayerMemoryShare.m_DamiCameraFollwer.
        //CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
        //if (lTempGameSceneWindow && lTempGameSceneWindow.GetShow())
        //    lTempGameSceneWindow.SetBouncingBedCount(m_MyGameManager.GetFloorBouncingBedBoxCount(m_MyMemoryShare.m_FloorNumber));
    }

    public void updateFollwer(){ m_MyPlayerMemoryShare.m_DamiCameraFollwer.SetPercent(m_MyPlayerMemoryShare.m_MySplineFollower.modifiedResult.percent); }

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

    public void PlayerMouseDown()
    {
        //if (!PlayerCtrl())
        //{
        //    if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        //    {
        //        m_AllState[(int)m_CurState].MouseDown();
        //    }
        //}

        if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
            m_AllState[(int)m_CurState].MouseDown();

        m_MyPlayerMemoryShare.m_bDown = true;
        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void PlayerMouseDrag()
    {
        //if (!PlayerCtrl())
        //    return;


        if (!m_MyPlayerMemoryShare.m_bDown)
            return;


        if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
            m_AllState[(int)m_CurState].MouseDrag();

        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void MouseDrag()
    {
        const float CfHalfWidth = 2.0f;
        const float CfTotleWidth = CfHalfWidth * 2.0f;
        float lTempMoveX = Input.mousePosition.x - m_MyPlayerMemoryShare.m_OldMouseDownPos.x;
        float lTempMoveRatio = TotleSpeedRatio;

        lTempMoveX = (lTempMoveX / Screen.width) * CfTotleWidth;
        Vector2 lTempOffset = MySplineFollower.motion.offset;
        lTempOffset.x += lTempMoveX * lTempMoveRatio;
        lTempOffset = Vector2.ClampMagnitude(lTempOffset, CfHalfWidth);

        MySplineFollower.motion.offset = lTempOffset;
    }

    public void PlayerMouseUp()
    {
        if (m_MyPlayerMemoryShare.m_bDown)
        {
            if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
                m_AllState[(int)m_CurState].MouseUp();

            m_MyPlayerMemoryShare.m_bDown = false;
            m_MyPlayerMemoryShare.m_OldMouseDownPos = Vector3.zero;
        }
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

        if (other.tag == "Door")
        {
            CDoorGroup lTempCDoorGroup = other.gameObject.GetComponentInParent<CDoorGroup>();
            lTempCDoorGroup.Show(false);


            CDoorGroup.ELDoorType lTempDoorDis;
            if (MySplineFollower.motion.offset.x < 0.0f)
            {
                lTempDoorDis = CDoorGroup.ELDoorType.eLDoor;
            }
            else
            {
                lTempDoorDis = CDoorGroup.ELDoorType.eRDoor;
            }

            CDoor lTempDoor = lTempCDoorGroup.GetDoor(lTempDoorDis);

            CGGameSceneData.EPlayAccessoriesType lTempPlayAccessoriesType = lTempDoor.PlayAccessoriesType;
            CGGameSceneData.EDoorType lTempType = lTempDoor.DoorType;

            int lTempAddLevel = lTempType == CGGameSceneData.EDoorType.eGood ? 1 : -1;

            CRoleAccessories lTempRoleAccessories = m_AllReplaceableAccessories[(int)lTempPlayAccessoriesType];
            lTempRoleAccessories.SetUpdateMat(lTempRoleAccessories.CurLevelIndex + lTempAddLevel);

            if (m_AnimatorStateCtl != null && lTempType == CGGameSceneData.EDoorType.eGood)
            {
                ((CHitStatePlayer)m_AllState[(int)StaticGlobalDel.EMovableState.eHit]).HitType = CHitStateBase.EHitType.eDoorGood;
                this.ChangState = StaticGlobalDel.EMovableState.eHit;
                this.SameStatusUpdate = true;
            }

            SetHpCount(CurHpCount + (lTempAddLevel * 3));
        }
        else if (other.tag == "Lipstick")
        {


            GameObject lTempObj = other.gameObject.transform.parent.gameObject;
            lTempObj.SetActive(false);

            SetHpCount(CurHpCount + 1);
        }
        else if (other.tag == "Mud")
        {
            if (m_AnimatorStateCtl != null)
            {
                ((CHitStatePlayer)m_AllState[(int)StaticGlobalDel.EMovableState.eHit]).HitType = CHitStateBase.EHitType.eBad;
                this.ChangState = StaticGlobalDel.EMovableState.eHit;
                this.SameStatusUpdate = true;
            }

            other.gameObject.SetActive(false);
            SetHpCount(CurHpCount - 1);
        }
        else if (other.tag == "End")
        {
            for (int i = 0; i < m_MyPlayerMemoryShare.m_AllHpBarObj.Length; i++)
                m_MyPlayerMemoryShare.m_AllHpBarObj[i].SetActive(false);


            m_MyPlayerMemoryShare.m_PlayerWinLoseCamera.gameObject.SetActive(true);
            m_MyGameManager.SetState(CGameManager.EState.eReadyEnd);
            m_MyMemoryShare.m_MySplineFollower.enabled = false;
            other.gameObject.SetActive(false);

            if (m_AnimatorStateCtl != null)
                this.ChangState = StaticGlobalDel.EMovableState.eWait;

            ShowEndFx(true);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        //if (other.tag == "BouncingBed")
        //    m_MyPlayerMemoryShare.m_TouchBouncingBed = null;

        //if (other.tag == "Mud")
        //{
        //    if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        //        m_AllState[(int)m_CurState].UpdateOriginalAnimation();
        //}
    }
}
