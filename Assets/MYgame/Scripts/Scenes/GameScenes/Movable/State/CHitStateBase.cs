using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitStateBase : CMovableStatePototype
{
    const float CHitTime = 0.3f;
  
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eHit; }

    public CHitStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        UpdateOriginalAnimation();
        //m_MyMemoryShare.m_MyMovable.SetMoveBuff(CMovableBase.ESpeedBuff.eHit, 0.01f);
    }

    protected override void updataState()
    {
        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.PlayingEnd)
                m_MyMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eMove;
        }
        else
        {
            if (m_StateTime >= CHitTime)
                m_MyMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eMove;
        }
    }

    protected override void OutState()
    {
        //m_MyMemoryShare.m_MyMovable.SetMoveBuff(CMovableBase.ESpeedBuff.eHit, 1.0f);
    }


    public override void UpdateOriginalAnimation()
    {
        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eHit);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        }

       
    }
}
