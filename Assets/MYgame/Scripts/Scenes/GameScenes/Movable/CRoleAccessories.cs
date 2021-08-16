using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CRoleAccessories : MonoBehaviour
{
    [SerializeField] protected CGGameSceneData.EPlayAccessoriesType m_MyAccessoriesType;
    [SerializeField] protected Material[] m_AllMat;
    [SerializeField] protected int m_CurLevelIndex = 1;
    public int CurLevelIndex { get { return m_CurLevelIndex; } }


    protected Renderer m_MyRenderer = null;
    public Renderer MyRenderer { get { return m_MyRenderer; } }

    private void Awake()
    {
        m_MyRenderer = this.GetComponent<Renderer>();
        m_MyRenderer.material = m_AllMat[m_CurLevelIndex];

        CPlayer lTempPlayer = this.gameObject.GetComponentInParent<CPlayer>();
        lTempPlayer.SetAllReplaceableAccessories(this, m_MyAccessoriesType);
    }

    public void SetUpdateMat(int SetCurLevelIndex)
    {
        if (0 > SetCurLevelIndex || SetCurLevelIndex >= m_AllMat.Length)
            return;

        m_CurLevelIndex = SetCurLevelIndex;
    }

    public void UpdateMat()
    {
        m_MyRenderer.material = m_AllMat[m_CurLevelIndex];
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
