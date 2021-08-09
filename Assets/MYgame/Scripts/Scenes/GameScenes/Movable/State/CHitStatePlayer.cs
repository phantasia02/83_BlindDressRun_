using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitStatePlayer : CHitStateBase
{
    CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    public CHitStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
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
        base.LateUpdate();
    }

    public override void MouseDrag()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.MouseDrag();

        base.MouseDrag();
    }
}
