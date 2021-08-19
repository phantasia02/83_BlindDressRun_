using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEndStateBase : CMovableStatePototype
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eEnd; }

    public CEndStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyMemoryShare.m_MySplineFollower.enabled = false;
        m_MyMemoryShare.m_MyMovable.EndDoTween.BuildSequence();
        m_MyMemoryShare.m_MyMovable.EndDoTween.PlayForward();
        
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {
        m_MyMemoryShare.m_MyMovable.ShowEndFx(true);
    }
}
