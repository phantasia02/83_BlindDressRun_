using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWinStatePlayer : CWinStateBase
{
    CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    public CWinStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();


        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eWin);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        }


    }

    protected override void updataState()
    {
        //if (m_OldStateTime < 3.0f && m_StateTime >= 3.0f)
        //{
        //    if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        //    {
        //        m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eWin);
        //        m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        //    }
        //}

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
