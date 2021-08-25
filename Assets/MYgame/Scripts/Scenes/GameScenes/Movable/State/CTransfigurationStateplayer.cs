using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTransfigurationStateplayer : CTransfigurationStateBase
{
    CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    public CTransfigurationStateplayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        m_MyPlayerMemoryShare.m_MyPlayer.MySplineFollower.follow = false;
        m_MyPlayerMemoryShare.m_MyPlayer.MySplineFollower.enabled = true;
        UpdateOriginalAnimation();
    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void LateUpdate()
    {
    }

    public override void UpdateOriginalAnimation()
    {
        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eRun);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        }
    }
}
