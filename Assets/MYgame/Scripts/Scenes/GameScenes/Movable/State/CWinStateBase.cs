using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWinStateBase : CMovableStatePototype
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eWin; }

    public CWinStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        //if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        //{
        //    m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eWin);
        //    m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        //}

        
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
