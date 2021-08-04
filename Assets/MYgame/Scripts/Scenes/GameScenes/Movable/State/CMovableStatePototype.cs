using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMovableStateData{}

public abstract class CMovableStatePototype
{

    protected CMovableBase m_MyMovable = null;
    protected CGameManager m_MyGameManager = null;
    protected CMemoryShareBase m_MyMemoryShare = null;


    abstract public StaticGlobalDel.EMovableState StateType();
    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;

    protected Vector3 m_v3DownPos = Vector3.zero;
    protected bool m_bDownOKPos = false;

    protected Vector3[] COffsetDownPos = new Vector3[4]{ Vector3.forward * 2.0f, Vector3.right * 2.0f, Vector3.forward * -2.0f, Vector3.right * -2.0f};

    public CMovableStatePototype(CMovableBase pamMovableBase)
    {
        if (pamMovableBase == null)
            return;

        
        m_MyMovable = pamMovableBase;
        m_MyGameManager = m_MyMovable.GetComponentInParent<CGameManager>();
        m_MyMemoryShare = pamMovableBase.MyMemoryShare;

  
    }

    public void ClearTime()
    {
        m_StateTime = 0.0f;
        m_StateCount = 0;
        m_StateUnscaledTime = 0.0f;
    }

    public void InMovableState()
    {
        InState();
        ClearTime();
    }

    public void updataMovableState()
    {
        m_StateTime += Time.deltaTime;
        m_StateCount++;
        m_StateUnscaledTime += Time.unscaledDeltaTime;
        updataState();
    }

    public void OutMovableState()
    {
        OutState();
    }


    protected virtual void InState()
    {

    }

    protected virtual void updataState()
    {

    }

    public virtual void LateUpdate()
    {
       
    }

    protected virtual void OutState()
    {

    }

    public virtual void Input(RaycastHit hit)
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {

    }

    public virtual void OnTriggerExit(Collider other)
    {

    }


    public virtual void OnCollisionEnter(Collision collision)
    {

    }

    public virtual void OnTriggerStay(Collider other)
    {
    }



    public virtual void MouseDown()
    {

    }


    public bool FloatingToFloorChack()
    {
        if (m_MyMemoryShare.m_MyMovable.transform.position.y - m_MyMemoryShare.m_OldPos.y >= 0.0f)
            return false;


        Vector3 TempMyFloorStartPos = m_MyMemoryShare.m_FloorRayStart.position;
        int lTempLayerMask = StaticGlobalDel.g_FloorLayerMask | StaticGlobalDel.g_WinLayerMask;


        if (Physics.Raycast(TempMyFloorStartPos, Vector3.down, out RaycastHit hit, 1.0f, lTempLayerMask))
        {
           // m_MyMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eWait;
            return true;
        }

        return false;
    }

}
