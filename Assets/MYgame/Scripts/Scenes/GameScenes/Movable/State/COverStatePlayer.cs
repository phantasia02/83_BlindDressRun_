using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class COverStatePlayer : COverStateBase
{
    CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    public COverStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {

        base.InState();


        CAnimatorStateCtl.EState lTempState = CAnimatorStateCtl.EState.eDeath;
        m_MyMemoryShare.m_MySplineFollower.enabled = false;
       // m_MyPlayerMemoryShare.m_PlayerNormalBuffCamera.gameObject.SetActive(false);

        if (m_MyGameManager.CurState == CGameManager.EState.eReadyEnd2 || m_MyGameManager.CurState == CGameManager.EState.eReadyEnd || m_MyGameManager.CurState == CGameManager.EState.eNextEnd)
            lTempState = CAnimatorStateCtl.EState.eIdle;
        else
            m_MyPlayerMemoryShare.m_MyPlayer.ShowHpBar(false);

        
        m_MyMemoryShare.m_MyMovable.transform.DOLocalRotate(new Vector3(0.0f, 180.0f, 0.0f), 3.0f);

        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(lTempState);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        }
    }

    protected override void updataState()
    {
        if (m_MyGameManager.CurState == CGameManager.EState.eReadyEnd2 || m_MyGameManager.CurState == CGameManager.EState.eReadyEnd || m_MyGameManager.CurState == CGameManager.EState.eNextEnd)
        {
            if (MomentinTime(3.0f))
            {
                if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
                {
                    m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eDeath);
                    m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
                    m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.ResetForward = false;
                }
            }

            if (MomentinTime(3.0f))
                m_MyGameManager.SetState(CGameManager.EState.eGameOver);
        }
        else
        {
            if (MomentinTime(3.0f))
                m_MyGameManager.SetState(CGameManager.EState.eGameOver);
        }


        base.updataState();
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void LateUpdate()
    {
    }
}
