using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CWinStatePlayer : CWinStateBase
{
    CPlayerMemoryShare m_MyPlayerMemoryShare = null;
    Vector3 m_EndForward = Vector3.zero;


    public CWinStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();


        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eIdle);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        }
        m_MyMemoryShare.m_MyMovable.transform.DOLocalRotate(new Vector3(0.0f, 180, 0.0f), 3.0f);
    }

    protected override void updataState()
    {

        if (MomentinTime(3.0f))
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eWin);
        }

        if (MomentinTime(6.0f))
            m_MyGameManager.SetState(CGameManager.EState.eWinUI);

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
