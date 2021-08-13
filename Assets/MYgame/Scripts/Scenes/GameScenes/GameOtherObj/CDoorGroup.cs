using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDoorGroup : MonoBehaviour
{
    public enum ELDoorType
    {
        eLDoor = 0,
        eRDoor = 1,
        eMax
    }


    [SerializeField] protected CDoor[] m_AllDoor = null;
    public CDoor GetDoor(ELDoorType index) { return m_AllDoor[(int)index]; }
    protected Collider m_Myprotected = null;
    protected Renderer[] m_AllRenderer = null;

    private void Awake()
    {
        m_Myprotected = this.GetComponentInChildren<Collider>();
        m_AllRenderer = this.GetComponentsInChildren<Renderer>();
    }


    public void Show(bool setshow)
    {
        float lTempAlphaval = 1.0f;
        if (setshow)
        {
            lTempAlphaval = 1.0f;
        }
        else
        {
            lTempAlphaval = 0.5f;
        }

        for (int i = 0; i < m_AllRenderer.Length; i++)
        {
            //Color lTemp = m_AllRenderer[i].material.color;
            //lTemp.a = lTempAlphaval;
            //m_AllRenderer[i].material.color = lTemp;
            m_AllRenderer[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < m_AllDoor.Length; i++)
            m_AllDoor[i].ShowAccessories(setshow);

        m_Myprotected.gameObject.SetActive(setshow);
    }


}
