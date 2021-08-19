using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEndStatePlayer : CEndStateBase
{
    CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    public CEndStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {

        base.InState();
        m_MyPlayerMemoryShare.m_PlayerWinLoseCamera.gameObject.SetActive(true);

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
