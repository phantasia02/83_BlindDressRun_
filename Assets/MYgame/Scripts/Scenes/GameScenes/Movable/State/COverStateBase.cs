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

    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
