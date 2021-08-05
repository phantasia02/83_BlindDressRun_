using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStatePlayer : CWaitStateBase
{
    CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    public CWaitStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {

        base.InState();
        m_MyPlayerMemoryShare.m_MyPlayer.MySplineFollower.follow = false;
        //m_MyPlayerMemoryShare.m_PlayerJumpCamera.gameObject.SetActive(false);

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
}
