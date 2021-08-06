using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CMoveStateBase : CMovableStatePototype
{
    protected float m_MoveDis = 0.0f;


    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eMove; }

    public CMoveStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
       
    }

    protected override void InState()
    {
        m_MoveDis = 0.0f;
        UpdateOriginalAnimation();
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }



    public override void UpdateOriginalAnimation()
    {
        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eRun);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
        }
    }
}
