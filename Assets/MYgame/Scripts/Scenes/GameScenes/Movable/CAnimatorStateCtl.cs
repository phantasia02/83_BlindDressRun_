using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CAnimatorStateCtl : MonoBehaviour
{
    public const float Jump1Speed = 0.5f;


    [System.Serializable]
    public class cAnimatorData
    {
        [SerializeField] public string m_flagName = "";
        [SerializeField] public string m_AnimationName = "";
        [SerializeField] public string m_AnimationStateName = "";
        [SerializeField] public float m_Speed = 1.0f;
        [HideInInspector] public AnimationClip m_AnimationClip = null;
        [HideInInspector] public float m_AnimationTime = 0.0f;
    }

    public enum EState
    {
        eIdle        = 0,
        eRun         = 1,
        eRun1        = 2,
        eRun2        = 3,
        eRun3        = 4,
        eRun4        = 5,
        eDeath       = 6,
        eWin         = 7,
        eHit         = 8,
        eMax
    }

    public class cAnimationCallBackPar
    {
        public string AnimationName     = "";
        public EState eAnimationState   = EState.eMax;
        public int iIndex = 0;
    }

    public delegate void ReturnAnimationCall(cAnimationCallBackPar Paramete);

    public Animator m_ThisAnimator = null;
    public float AnimatorSpeed
    {
        set { m_ThisAnimator.speed = value; }
        get { return m_ThisAnimator.speed; }
    }

    [SerializeField] CMovableBase m_MyMovableBase = null;

    
    [VarRename(new string[] { "Idle", "Run", "Run1", "Run2", "Run3", "Run4", "Death", "Win", "Hit"})]
    [SerializeField] public cAnimatorData[] m_AllAnimatorData = new cAnimatorData[(int)EState.eMax];

    public ReturnAnimationCall m_EndCallBack = null;
    public ReturnAnimationCall m_KeyFramMessageCallBack = null;

    [VarRename(new string[] { "Idle0", "Idle1", "Idle2"})]
    [SerializeField] public cAnimatorData[] m_AllBaseIdleAnima = new cAnimatorData[2];

    bool m_PlayingEnd = false;
    public bool PlayingEnd{get { return m_PlayingEnd; }}

    EState m_CurState = EState.eIdle;
    public EState CurState { get { return m_CurState; } }
    int m_IdleIndex = 0;


    protected void Awake()
    {
        m_ThisAnimator = gameObject.GetComponent<Animator>();
        m_MyMovableBase = gameObject.GetComponentInParent<CMovableBase>();

        if (m_MyMovableBase)
            m_MyMovableBase.AnimatorStateCtl = this;

        if (m_ThisAnimator)
        {
            for (int i = 0; i < m_AllAnimatorData.Length; i++)
                InitAnimatorData(ref m_AllAnimatorData[i]);

            for (int i = 0; i < m_AllBaseIdleAnima.Length; i++)
                InitAnimatorData(ref m_AllBaseIdleAnima[i]);
        }

        
    }

    public void OnEnable()
    {

    }

    protected void InitAnimatorData(ref cAnimatorData rAnidata)
    {
        if (rAnidata.m_AnimationName.Length == 0)
            return;

        RuntimeAnimatorController ac = m_ThisAnimator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
            if (ac.animationClips[i].name == rAnidata.m_AnimationName)
            {
                rAnidata.m_AnimationClip = ac.animationClips[i];
                rAnidata.m_AnimationTime = rAnidata.m_AnimationClip.length;
                
            }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_ThisAnimator)
            return;

        if (m_AllAnimatorData[(int)m_CurState].m_AnimationName.Length == 0)
            return;

        AnimatorStateInfo info = m_ThisAnimator.GetCurrentAnimatorStateInfo(0);

        if (info.speed == 0)
            return;

        if (info.normalizedTime >= (1.0f/ info.speed) && info.IsName(m_AllAnimatorData[(int)m_CurState].m_AnimationStateName) && !m_PlayingEnd)
        {
            if (m_EndCallBack != null)
            {
                cAnimationCallBackPar lTempAnimationCallBackPar = new cAnimationCallBackPar();
                lTempAnimationCallBackPar.eAnimationState = m_CurState;
                m_EndCallBack(lTempAnimationCallBackPar);
                m_EndCallBack = null;
            }

            m_PlayingEnd = true;
        }

    }

    public float GetCurrentNormalizedTime()
    {
        Debug.Log(m_ThisAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));


        return m_ThisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

    }
    public string GetCurCurrentAnimator(){return m_ThisAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name; }

    public float GetAnimationStateTime(EState pGetstateTime)
    {
        if (m_AllAnimatorData[(int)pGetstateTime].m_Speed == 0.0f || AnimatorSpeed == 0.0f)
            return 0.0f;

        //return m_AllAnimatorData[(int)pGetstateTime].m_AnimationTime;
        return (m_AllAnimatorData[(int)pGetstateTime].m_AnimationTime / m_AllAnimatorData[(int)pGetstateTime].m_Speed) / AnimatorSpeed;
    }

    public float GetAnimatiotStateTime(EState setstate)
    {
        int iCurStatIndex = (int)setstate;

        if (iCurStatIndex < 0 || iCurStatIndex >= (int)EState.eMax)
            return 0.0f;

        return m_AllAnimatorData[iCurStatIndex].m_AnimationTime;
    }

    public void OpenAnimatorComponent(bool open)
    {
        if (!m_ThisAnimator || m_ThisAnimator.enabled == open)
            return;

        m_ThisAnimator.enabled = open;
    }

    public void SetBaseIdleIndex(int SetIdelIndex)
    {
        if (!m_ThisAnimator)
            return;

        int iCurIdelIndex = SetIdelIndex;


        if (iCurIdelIndex < 0 || iCurIdelIndex >= m_AllBaseIdleAnima.Length)
            return;

        if (m_IdleIndex == SetIdelIndex)
            return;

        int ioldIdleIndex = m_IdleIndex;
        m_IdleIndex = iCurIdelIndex;

        //if (m_AllBaseIdleAnima[ioldIdleIndex].m_flagName.Length != 0)
        //    m_ThisAnimator.SetInteger(m_AllBaseIdleAnima[ioldIdleIndex].m_flagName, false);

        if (m_AllBaseIdleAnima[iCurIdelIndex].m_flagName.Length != 0)
            m_ThisAnimator.SetInteger(m_AllBaseIdleAnima[iCurIdelIndex].m_flagName, iCurIdelIndex);
    }

    public void SetCurState(EState SetState)
    {
        if (!m_ThisAnimator)
            return;

        int iCurStatIndex = (int)SetState;

        if (iCurStatIndex < 0 || iCurStatIndex >= (int)EState.eMax)
            return;

        if (m_CurState == SetState)
            return;

        EState oldState = m_CurState;
        m_CurState = SetState;
        int ioldStateIndex = (int)oldState;

        if (m_AllAnimatorData[ioldStateIndex].m_flagName.Length != 0)
            m_ThisAnimator.ResetTrigger(m_AllAnimatorData[ioldStateIndex].m_flagName);

        if (m_AllAnimatorData[iCurStatIndex].m_flagName.Length != 0)
        {
            m_ThisAnimator.gameObject.transform.eulerAngles = Vector3.zero;

            m_ThisAnimator.SetTrigger(m_AllAnimatorData[iCurStatIndex].m_flagName);
            m_PlayingEnd = false;
        }
    }

    public void KeyFrameCall(int setmessageIndex)
    {
        //Debug.Log("setmessageIndex = " + setmessageIndex.ToString());
        if (m_KeyFramMessageCallBack != null)
        {
            cAnimationCallBackPar lTempAnimationCallBackPar = new cAnimationCallBackPar();
            lTempAnimationCallBackPar.eAnimationState = m_CurState;
            lTempAnimationCallBackPar.iIndex = setmessageIndex;
            lTempAnimationCallBackPar.AnimationName = m_AllAnimatorData[(int)m_CurState].m_AnimationStateName;
            m_KeyFramMessageCallBack(lTempAnimationCallBackPar);
            
        }
    }
}
