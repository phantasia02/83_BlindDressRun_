using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMovableCollisionBaseData : CMovableStateData
{
    public CHitStateBase.EHitType m_HitType = CHitStateBase.EHitType.eBad;
}

public class CHitStateBase : CMovableStatePototype
{
    public enum EHitType
    {
        eBad        = 0,
        eDoorPose   = 1,
        eNormalGood = 2,
        eMax
    }

    const float CHitTime = 0.3f;
    protected EHitType m_HitType = EHitType.eBad;
    public EHitType HitType
    {
        set { m_HitType = value; }
        get { return m_HitType; }
    }


    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eHit; }

    public CHitStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        UpdateOriginalAnimation();
        //m_MyMemoryShare.m_MyMovable.SetMoveBuff(CMovableBase.ESpeedBuff.eHit, 0.01f);

        m_MyMemoryShare.m_MyMovable.SetMoveBuff(CMovableBase.ESpeedBuff.eHit, 0.01f);


    }

    protected override void updataState()
    {
        if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl != null)
        {
            if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.PlayingEnd)
                m_MyMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eMove;
            else
            {
                
               // if (m_StateTime >= CHitTime)
               if (m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.PlayingEnd)
                    m_MyMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eMove;
            }
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
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eHit, (int)EHitType.eBad);
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.AnimatorSpeed = 1.0f;
            m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.ResetForward = false;
        }
    }
}
