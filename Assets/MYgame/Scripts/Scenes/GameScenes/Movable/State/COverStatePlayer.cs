using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    protected override void updataState()
    {
        if (m_OldStateTime < 3.0f && m_StateTime >= 3.0f)
            m_MyGameManager.SetState(CGameManager.EState.eGameOver);

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
