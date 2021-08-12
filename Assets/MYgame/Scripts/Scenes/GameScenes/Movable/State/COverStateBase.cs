using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COverStateBase : CMovableStatePototype
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eOver; }

    public COverStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eDeath);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.ResetForward = false;
        }

        m_MyMemoryShare.m_MySplineFollower.enabled = false;
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
