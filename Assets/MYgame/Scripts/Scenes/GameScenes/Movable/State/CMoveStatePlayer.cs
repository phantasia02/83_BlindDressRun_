using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStatePlayer : CMoveStateBase
{
    CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    public CMoveStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        m_MyPlayerMemoryShare.m_MyPlayer.MySplineFollower.follow = true;
        //m_MyPlayerMemoryShare.m_PlayerJumpCamera.Priority = 1;
        //m_MyPlayerMemoryShare.m_PlayerNormalCamera.Priority = 2;
        //m_MyPlayerMemoryShare.m_PlayerJumpCamera.gameObject.SetActive(false);
    }

    protected override void updataState()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.updateFollwer();

        base.updataState();
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void LateUpdate()
    {

    }

    public override void MouseDrag()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.MouseDrag();

        base.MouseDrag();
    }
}
