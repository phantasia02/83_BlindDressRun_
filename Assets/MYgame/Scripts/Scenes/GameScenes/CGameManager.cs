using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CEndNpc
{
    public Animator                     m_MyAnimator     = null;
    public HairSelector.HairSelector    m_MyHairSelector = null;
}


public class CGameManager : MonoBehaviour
{
    public enum EState
    {
        eReady      = 0,
        ePlay       = 1,
        eReadyEnd   = 2,
        eNextWin    = 3,
        eGameOver   = 4,
        eWinUI      = 5,
        eMax
    };

    public enum EQAData
    {
        eQ_DataIndex = 0,
        eA_DataIndex = 1,
        eMax
    };

    bool m_bInitOK = false;
    bool m_bDown = false;

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    protected ResultUI m_MyResultUI = null;
    public ResultUI MyResultUI { get { return m_MyResultUI; } }

    protected Camera m_Camera = null;
    public Camera MainCamera { get { return m_Camera; } }

    [SerializeField] protected GameObject m_DamiCameraFollwer;
    public GameObject DamiCameraFollwer { get { return m_DamiCameraFollwer; } }

    protected CPlayer m_Player = null;
    public CPlayer Player { get { return m_Player; } }

    private EState m_eCurState = EState.eReady;
    public EState CurState { get { return m_eCurState; } }
    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;
    protected Vector3 m_OldInput;
    protected float m_HalfScreenWidth = 600.0f;

    protected List<CMovableBase>    m_AllMovable    = new List<CMovableBase>();
    protected List<CEndNpc>         m_AllEndNpc     = new List<CEndNpc>();
    //HairSelector

    void Awake()
    {
        

        Application.targetFrameRate = 60;
        const float HWRatioPototype = 2688.0f / 1242.0f;
        float lTempNewHWRatio = ((float)Screen.height / (float)Screen.width);
        m_HalfScreenWidth = (1242.0f / 2.0f) * (lTempNewHWRatio / HWRatioPototype);

        m_MyResultUI = gameObject.GetComponentInChildren<ResultUI>();

        HairSelector.HairSelector[] lTempHairSelector = GetComponentsInChildren<HairSelector.HairSelector>();
        for (int i = 0; i < lTempHairSelector.Length; i++)
        {
            CEndNpc lTempEndNpc = new CEndNpc();
            lTempEndNpc.m_MyAnimator    = lTempHairSelector[i].GetComponent<Animator>();
            lTempEndNpc.m_MyHairSelector = lTempHairSelector[i];
            m_AllEndNpc.Add(lTempEndNpc);
           // lTempEndNpc.m_MyHairSelector.SetShowObj(Random.Range(0, lTempEndNpc.m_MyHairSelector.Hairstyles.Length));
        }

        // Debug.Log("EnemyhitOK = " + LayerMask.GetMask("EnemyhitOK").ToString());


            //for (int i = 0; i < m_AllFloorGroup.Count; i++)
            //{
            //    m_AllFloorGroup[i].GetComponentsInChildren<CBouncingBedBox>();
            //}

    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            GameObject lTempCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
            if (lTempCameraObj != null)
                m_Camera = lTempCameraObj.GetComponent<Camera>();

            if (m_Player == null)
                m_Player = gameObject.GetComponentInChildren<CPlayer>();

            if (m_Camera != null && m_Player != null)
            {
                m_bInitOK = true;
            }

            yield return null;

        } while (!m_bInitOK);

     
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_bInitOK)
            return;

  
        m_StateTime += Time.deltaTime;
        m_StateCount++;
        m_StateUnscaledTime += Time.unscaledDeltaTime;

        switch (m_eCurState)
        {
            case EState.eReady:
                {
                    UsePlayTick();
                }
                break;
            case EState.ePlay:
                {
                    UsePlayTick();
                }
                break;
            case EState.eReadyEnd:
                {
                    //WinStateAICheatingTime();
                    if (m_StateTime >= 3.0f)
                        SetState(EState.eNextWin);
                }
                break;
            case EState.eNextWin:
                {
                    //WinStateAICheatingTime();
                    if (m_StateTime >= 3.0f)
                        SetState(EState.eWinUI);

                }
                break;
            case EState.eGameOver:
                {

                }
                break;
        }
    }

    public void LateUpdate()
    {

        //m_PlayerFollowObj.transform.position = m_Player.transform.position;
        //Vector3 lTempV3 = m_Player.transform.position;
        //lTempV3.y += 15.0f;
        //lTempV3.z += -7.5f;
        //m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, lTempV3, 0.95f);
    }


    public void SetState(EState lsetState)
    {
        if (lsetState == m_eCurState)
            return;

        EState lOldState = m_eCurState;
        m_StateTime = 0.0f;
        m_StateCount = 0;
        m_StateUnscaledTime = 0.0f;
        m_eCurState = lsetState;


        switch (m_eCurState)
        {
            case EState.eReady:
                {
                }
                break;
            case EState.ePlay:
                {
                    //m_Player.SetCurState(StaticGlobalDel.EMovableState.eMove);

                    //for (int i = 0; i < m_AllCNPC.Count; i++)
                    //    m_AllCNPC[i].SetCurState(StaticGlobalDel.EMovableState.eMove);
                }
                break;
            case EState.eReadyEnd:
                {
                    string lTempAnimationName = "win";

                    if (Player.CurHpCount < StaticGlobalDel.g_DefHp)
                        lTempAnimationName = "loss";

                    for (int i = 0; i < m_AllEndNpc.Count; i++)
                        m_AllEndNpc[i].m_MyAnimator.SetTrigger(lTempAnimationName);


                }
                break;
            case EState.eNextWin:
                {
                    if (Player.CurHpCount < StaticGlobalDel.g_DefHp)
                        Player.ChangState = StaticGlobalDel.EMovableState.eOver;
                    else
                        Player.ChangState = StaticGlobalDel.EMovableState.eWin;

                    CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
                    if (lTempGameSceneWindow)
                        lTempGameSceneWindow.ShowObj(false);
                }
                break;
            case EState.eWinUI:
                {
                    m_MyResultUI.ShowSuccessUI();
                }
                break;
            case EState.eGameOver:
                {
                    m_MyResultUI.ShowFailedUI();
                }
                break;
        }
    }

    public void UsePlayTick()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            
            m_bDown = true;
            m_OldInput = Input.mousePosition;
            //InputRay();
        }
        else if (Input.GetMouseButton(0))
        {
            //float moveX = (Input.mousePosition.x - m_OldInput.x) / m_HalfScreenWidth;
            //m_Player.SetXMove(moveX);
            //m_OldInput = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (m_bDown)
            {
                m_OldInput = Vector3.zero;
                m_bDown = false;
            }
        }

#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            bool isTouchUIElement = EventSystem.current.IsPointerOverGameObject(touch.fingerId);

            if (!isTouchUIElement)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        {
                            m_bDown = true;
                            m_OldInput = Input.mousePosition;
                        }
                        break;
                    case TouchPhase.Moved:
                        {
                            m_OldInput = Input.mousePosition;
                        }
                        break;
                    case TouchPhase.Ended:
                        {
                            if (m_bDown)
                            {
                                m_bDown = false;
                            }
                        }
                        break;

                }
            }
        }
#endif


        if (m_eCurState == EState.eReady)
        {
            if (m_bDown)
            {
                SetState(EState.ePlay);

                CReadyGameWindow lTempCReadyGameWindow = CReadyGameWindow.SharedInstance;
                if (lTempCReadyGameWindow && lTempCReadyGameWindow.GetShow())
                    lTempCReadyGameWindow.CloseShowUI();

                CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
                if (lTempGameSceneWindow && !lTempGameSceneWindow.GetShow())
                {
                    m_Player.ChangState = StaticGlobalDel.EMovableState.eMove;
                    lTempGameSceneWindow.ShowObj(true);
                }
            }
        }

    }

    public void InputRay()
    {
        
    }
    
    
    public bool RemoveMovable(CMovableBase setMovable)
    {
        bool lTemp = m_AllMovable.Remove(setMovable);

        //CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
        //if (lTempGameSceneWindow && !lTempGameSceneWindow.GetShow())
        //    lTempGameSceneWindow.SetRivalNumber(m_AllMovable.Count);

        return lTemp;
    }

    public void OnNext()
    {
        m_ChangeScenes.LoadGameScenes();
    }

    public void OnReset()
    {
        m_ChangeScenes.ResetScene();
    }

}
