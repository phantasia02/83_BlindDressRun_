using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CRoleAccessories : MonoBehaviour
{
    [SerializeField] protected CGGameSceneData.EPlayAccessoriesType m_MyAccessoriesType;
    [SerializeField] protected CGGameSceneData.EDoorType            m_QualityType;
   // [SerializeField] protected Material[] m_AllMat;
  //  [SerializeField] protected int m_CurLevelIndex = 1;
  //  public int CurLevelIndex { get { return m_CurLevelIndex; } }

    
    protected Renderer m_MyRenderer = null;
    protected CPlayer m_MyPlayer = null;
    public Renderer MyRenderer { get { return m_MyRenderer; } }

    private void Awake()
    {
        m_MyRenderer = this.GetComponent<Renderer>();
        m_MyPlayer = this.gameObject.GetComponentInParent<CPlayer>();
        //  m_MyRenderer.material = m_AllMat[m_CurLevelIndex];

        //lTempPlayer.SetAllReplaceableAccessories(this, m_MyAccessoriesType);
    }

    public void Start()
    {
        m_MyPlayer.AddAllReplaceableAccessories(this.gameObject, m_MyAccessoriesType, m_QualityType);

        if (m_QualityType !=  CGGameSceneData.EDoorType.eNormal)
            this.gameObject.SetActive(false);
    }

    public void SetUpdateMat(int SetCurLevelIndex)
    {

        //if (0 > SetCurLevelIndex || SetCurLevelIndex >= m_AllMat.Length)
        //    return;

        //m_CurLevelIndex = SetCurLevelIndex;
    }

    public void UpdateMat()
    {
      //  m_MyRenderer.material = m_AllMat[m_CurLevelIndex];
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
