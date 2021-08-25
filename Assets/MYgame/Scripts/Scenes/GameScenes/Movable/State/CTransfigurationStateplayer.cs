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

        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eRun);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        }

        m_MyPlayerMemoryShare.m_MyPlayTransfiguration.BuildSequence();
        m_MyPlayerMemoryShare.m_MyPlayTransfiguration.PlayForward();
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


    public void SetPose()
    {
        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eHit, (int)CHitStateBase.EHitType.eDoorPose);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.ResetForward = false;
           
        }
    }
}
