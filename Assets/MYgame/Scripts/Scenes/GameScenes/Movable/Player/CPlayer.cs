using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Dreamteck.Splines;
using DG.Tweening;

public class CPlayerMemoryShare : CMemoryShareBase
{
    public bool                     m_bDown                 = false;
    public Vector3                  m_OldMouseDownPos       = Vector3.zero;
    public CinemachineVirtualCamera m_PlayerNormalCamera    = null;
    public CinemachineVirtualCamera m_PlayerWinLoseCamera   = null;
    public Vector3                  m_OldMouseDragDirNormal = Vector3.zero;
    public SplineFollower           m_DamiCameraFollwer     = null;
    public CPlayer                  m_MyPlayer              = null;
    public GameObject[]             m_AllHpBarObj           = null;
    public CTweenSequence           m_MyPlayTransfiguration = null;
};

public class CPlayer : CMovableBase
{


    protected float m_MaxMoveDirSize = 0;

    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    [SerializeField] CinemachineVirtualCamera m_PlayerNormalCamera;
    public CinemachineVirtualCamera PlayerNormalFollowObj { get { return m_PlayerNormalCamera; } }

    [SerializeField] CinemachineVirtualCamera m_PlayerNormalBuffCamera;
    public CinemachineVirtualCamera PlayerNormalBuffCamera { get { return m_PlayerNormalBuffCamera; } }

    [SerializeField] CinemachineVirtualCamera m_PlayerWinLoseCamera;
    public CinemachineVirtualCamera PlayerWinLoseCamera { get { return m_PlayerWinLoseCamera; } }

    //[SerializeField] CRoleAccessories[] m_AllReplaceableAccessories;
    //public void SetAllReplaceableAccessories(CRoleAccessories setRoleAccessories, CGGameSceneData.EPlayAccessoriesType lPlayAccessoriesType)
    //{m_AllReplaceableAccessories[(int)lPlayAccessoriesType] = setRoleAccessories;}


    [SerializeField] List<List<List<GameObject>>> m_MyAccessories = new List<List<List<GameObject>>>((int)CGGameSceneData.EPlayAccessoriesType.eMax);
    public void AddAllReplaceableAccessories(GameObject setRoleAccessories, CGGameSceneData.EPlayAccessoriesType lPlayAccessoriesType, CGGameSceneData.EDoorType QualityType)
    { m_MyAccessories[(int)lPlayAccessoriesType][(int)QualityType].Add(setRoleAccessories); }

    
    CGGameSceneData.EDoorType[] m_CurQualityType = new CGGameSceneData.EDoorType[(int)CGGameSceneData.EPlayAccessoriesType.eMax];

    CGGameSceneData.EPlayAccessoriesType m_BuffPlayAccessoriesType;
    CGGameSceneData.EDoorType m_BuffQualityType;
    CDoorGroup m_BuffDoorGroup = null;
    CDoor m_BuffDoorObj = null;

    protected Vector3 m_OldMouseDragDir = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        CreateMemoryShare();

        m_AllState[(int)StaticGlobalDel.EMovableState.eWait]            = new CWaitStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove]            = new CMoveStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eHit]             = new CHitStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eWin]             = new CWinStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eOver]            = new COverStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eEnd]             = new CEndStatePlayer(this);
        m_AllState[(int)StaticGlobalDel.EMovableState.eTransfiguration] = new CTransfigurationStateplayer(this);

        AnimatorStateCtl.m_KeyFramMessageCallBack = AnimationCallBack;

        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 10.0f;

        List<List<GameObject>> lTempListGameObject = null;

        for (int i = 0; i < (int)CGGameSceneData.EPlayAccessoriesType.eMax; i++)
        {
            m_MyAccessories.Add(new List<List<GameObject>>());
            lTempListGameObject = m_MyAccessories[i];

            for (int x = 0; x < (int)CGGameSceneData.EDoorType.eMax; x++)
                lTempListGameObject.Add(new List<GameObject>());
        }


        m_CurQualityType[(int)CGGameSceneData.EPlayAccessoriesType.eClothing]   = CGGameSceneData.EDoorType.eNormal;
        m_CurQualityType[(int)CGGameSceneData.EPlayAccessoriesType.eShoe]       = CGGameSceneData.EDoorType.eNormal;

      //  const int CDefMatIndex = 1;
      //for (int i = 0; i < m_AllReplaceableAccessories.Length; i++)
      //    m_AllReplaceableAccessories[i].SetUpdateMat(CDefMatIndex);

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
        m_MyPlayerMemoryShare.m_AllHpBarObj         = m_AllHpbarObj;
        m_MyPlayerMemoryShare.m_DamiCameraFollwer   = m_MyGameManager.DamiCameraFollwer.GetComponent<SplineFollower>();
        m_MyPlayerMemoryShare.m_MyPlayer            = this;

        CTweenSequence[] lTempCTweenSequence =  this.GetComponentsInChildren<CTweenSequence>();
        for (int i = 0; i < lTempCTweenSequence.Length; i++)
        {
            if (lTempCTweenSequence[i].name == "PlayTransfiguration")
                m_MyPlayerMemoryShare.m_MyPlayTransfiguration = lTempCTweenSequence[i];
        }

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
        const float CfHalfWidth = 3.0f;
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
            m_BuffDoorGroup = other.gameObject.GetComponentInParent<CDoorGroup>();
            //lTempCDoorGroup.Show(false);
            m_BuffDoorGroup.ShowCollider(false);

            CDoorGroup.ELDoorType lTempDoorDis;
            if (MySplineFollower.motion.offset.x < 0.0f)
                lTempDoorDis = CDoorGroup.ELDoorType.eLDoor;
            else
                lTempDoorDis = CDoorGroup.ELDoorType.eRDoor;

            m_BuffDoorObj = m_BuffDoorGroup.GetDoor(lTempDoorDis);

            m_BuffPlayAccessoriesType = m_BuffDoorObj.PlayAccessoriesType;
            m_BuffQualityType = m_BuffDoorObj.DoorType;

            
            
            //CRoleAccessories lTempRoleAccessories = m_AllReplaceableAccessories[(int)lTempPlayAccessoriesType];
            //lTempRoleAccessories.SetUpdateMat(lTempRoleAccessories.CurLevelIndex + lTempAddLevel);
            //m_BuffRoleAccessories = lTempRoleAccessories;
            //m_BuffPlayAccessoriesType = lTempPlayAccessoriesType;
            //m_BuffQualityType = lTempType;

            //if (m_AnimatorStateCtl != null && m_BuffQualityType == CGGameSceneData.EDoorType.eGood)
            //if (m_AnimatorStateCtl != null)
            //{

            //}
            //if (lTempType == CGGameSceneData.EDoorType.eBad)
            //    lTempRoleAccessories.UpdateMat();
            m_MyPlayerMemoryShare.m_MyPlayTransfiguration.m_ListDOTween[0].m_TargetTransform = m_BuffDoorObj.TargetPos;
            
            this.ChangState = StaticGlobalDel.EMovableState.eTransfiguration;

            m_PlayerNormalBuffCamera.gameObject.SetActive(true);




        }
        else if (other.tag == "Lipstick")
        {
            GameObject lTempObj = other.gameObject.transform.parent.gameObject;
            lTempObj.SetActive(false);

            SetHpCount(CurHpCount + 1);
            m_FxParent[(int)EFxParentType.eSpine].transform.NewFxAddParentShow(CGGameSceneData.EAllFXType.eFlareLipstick);
        }
        else if (other.tag == "Mud")
        {
            if (m_AnimatorStateCtl != null)
            {
                ((CHitStatePlayer)m_AllState[(int)StaticGlobalDel.EMovableState.eHit]).HitType = CHitStateBase.EHitType.eBad;
                this.ChangState = StaticGlobalDel.EMovableState.eHit;
                this.SameStatusUpdate = true;
            }

            m_FxParent[(int)EFxParentType.eSpine].transform.NewFxAddParentShow(CGGameSceneData.EAllFXType.eUglyTemp);
            other.gameObject.SetActive(false);
            SetHpCount(CurHpCount - 1);
        }
        else if (other.tag == "End")
        {
            ShowHpBar(false);

           
            m_MyGameManager.SetState(CGameManager.EState.eReadyEnd);
            other.gameObject.SetActive(false);

            //if (m_AnimatorStateCtl != null)
            //{
            //    //if (CurHpCount < StaticGlobalDel.g_DefHp)
            //    //    ChangState = StaticGlobalDel.EMovableState.eOver;
            //    //else
            //    ChangState = StaticGlobalDel.EMovableState.eEnd;
            //}
            //    this.ChangState = StaticGlobalDel.EMovableState.eWait;
        }
    }

    public void ShowHpBar(bool show)
    {
        for (int i = 0; i < m_MyPlayerMemoryShare.m_AllHpBarObj.Length; i++)
            m_MyPlayerMemoryShare.m_AllHpBarObj[i].SetActive(false);
    }

    public override void OnTriggerExit(Collider other)
    {
    }

    public void AnimationCallBack(CAnimatorStateCtl.cAnimationCallBackPar CallbackReturn)
    {
        if (CallbackReturn.eAnimationState == CAnimatorStateCtl.EState.eHit && CallbackReturn.StateIndividualIndex == 1)
        {
            int shPropColorID = Shader.PropertyToID("_EmissionColor");
            Material lTempMaterial = null;
            Renderer lTempRenderer = null;
            GameObject lTempgameobj = null;
            if (CallbackReturn.iIndex == 0)
            {
                //for (int i = 0; i < m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_CurQualityType[(int)m_BuffPlayAccessoriesType]].Count; i++)
                //{
                //    lTempgameobj = m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_CurQualityType[(int)m_BuffPlayAccessoriesType]][i];
                //    lTempRenderer = lTempgameobj.GetComponent<Renderer>();
                //    lTempMaterial = lTempRenderer.material;
                //    lTempMaterial.EnableKeyword("_EMISSION");
                //    lTempMaterial.DOColor(m_HappyChangeColor, shPropColorID, 0.2f);
                //}
                //Time.timeScale = 0.1f;
            }
            else if (CallbackReturn.iIndex == 1)
            {
                for (int i = 0; i < m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_CurQualityType[(int)m_BuffPlayAccessoriesType]].Count; i++)
                {
                    lTempgameobj = m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_CurQualityType[(int)m_BuffPlayAccessoriesType]][i];
                    lTempRenderer = lTempgameobj.GetComponent<Renderer>();
                    lTempMaterial = lTempRenderer.material;
                    //lTempMaterial.DisableKeyword("_EMISSION");
                    //lTempMaterial.SetColor(shPropColorID, Color.black);
                    lTempgameobj.SetActive(false);
                }

                if (m_BuffQualityType == CGGameSceneData.EDoorType.eGood)
                    m_FxParent[(int)EFxParentType.eSpine].transform.NewFxAddParentShow(CGGameSceneData.EAllFXType.eFlareGoodDoor);
                else if (m_BuffQualityType == CGGameSceneData.EDoorType.eBad)
                    m_FxParent[(int)EFxParentType.eSpine].transform.NewFxAddParentShow(CGGameSceneData.EAllFXType.eUglyTemp);

                for (int i = 0; i < m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_BuffQualityType].Count; i++)
                {
                    lTempgameobj = m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_BuffQualityType][i];
                    lTempRenderer = lTempgameobj.GetComponent<Renderer>();
                    lTempMaterial = lTempRenderer.material;
                    lTempgameobj.SetActive(true);
                    //lTempMaterial.EnableKeyword("_EMISSION");
                    //lTempMaterial.SetColor(shPropColorID, m_HappyChangeColor);
                    //lTempMaterial.DOColor(Color.black, shPropColorID, 1.0f);
                }

            }
            else if (CallbackReturn.iIndex == 3)
            {
                m_CurQualityType[(int)m_BuffPlayAccessoriesType] = m_BuffQualityType;
            }
        }
    }


    public void ChangEndAnimation()
    {
        if (CurHpCount < StaticGlobalDel.g_DefHp)
            ChangState = StaticGlobalDel.EMovableState.eOver;
        else
            ChangState = StaticGlobalDel.EMovableState.eWin;
    }

    public void PlayTransfiguration(int eventnumber)
    {
        if (eventnumber == 0)
            m_BuffDoorObj.PlayAnimation();
        else if (eventnumber == 1)
        {
            ((CTransfigurationStateplayer)m_AllState[(int)StaticGlobalDel.EMovableState.eTransfiguration]).SetPose();
            m_BuffDoorObj.ShowmAccessoriesObj(false);

            if (m_BuffQualityType == CGGameSceneData.EDoorType.eGood)
                m_FxParent[(int)EFxParentType.eSpine].transform.NewFxAddParentShow(CGGameSceneData.EAllFXType.eFlareGoodDoor);
            else if (m_BuffQualityType == CGGameSceneData.EDoorType.eBad)
                m_FxParent[(int)EFxParentType.eSpine].transform.NewFxAddParentShow(CGGameSceneData.EAllFXType.eUglyTemp);


            int shPropColorID = Shader.PropertyToID("_EmissionColor");
            Material lTempMaterial = null;
            Renderer lTempRenderer = null;
            GameObject lTempgameobj = null;

            for (int i = 0; i < m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_CurQualityType[(int)m_BuffPlayAccessoriesType]].Count; i++)
            {
                lTempgameobj = m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_CurQualityType[(int)m_BuffPlayAccessoriesType]][i];
                lTempRenderer = lTempgameobj.GetComponent<Renderer>();
                lTempMaterial = lTempRenderer.material;
                lTempgameobj.SetActive(false);
            }

            for (int i = 0; i < m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_BuffQualityType].Count; i++)
            {
                lTempgameobj = m_MyAccessories[(int)m_BuffPlayAccessoriesType][(int)m_BuffQualityType][i];
                lTempRenderer = lTempgameobj.GetComponent<Renderer>();
                lTempMaterial = lTempRenderer.material;
                lTempgameobj.SetActive(true);
            }

            int lTempAddLevel = m_BuffQualityType == CGGameSceneData.EDoorType.eGood ? 1 : -1;
            SetHpCount(CurHpCount + (lTempAddLevel * 3));
        }
        //else if (eventnumber == 2)
        //{
        //    m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
        //}
        else if (eventnumber == 3)
        {
            Vector2 lTempOffset = MySplineFollower.motion.offset;
            lTempOffset.x = MySplineFollower.motion.offset.x < 0.0f ? -1.8f : 1.8f;
            MySplineFollower.motion.offset = lTempOffset;

            SplineSample lTempSplineSample = m_MyPlayerMemoryShare.m_MySplineFollower.spline.Project(this.transform.position);
            m_MyPlayerMemoryShare.m_MySplineFollower.SetPercent(lTempSplineSample.percent);

            updateFollwer();

            m_BuffDoorGroup.Show(false);
            m_PlayerNormalBuffCamera.gameObject.SetActive(false);
            ChangState = StaticGlobalDel.EMovableState.eMove;

            m_BuffDoorGroup = null;
            m_BuffDoorObj = null;
        }
    }

}
