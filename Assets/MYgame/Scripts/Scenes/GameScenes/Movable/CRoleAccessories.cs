using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CRoleAccessories : MonoBehaviour
{
    [SerializeField] protected Material[] m_AllMat;
    [SerializeField] protected int m_CurLevelIndex = 1;
    public int CurLevelIndex { get { return m_CurLevelIndex; } }

    protected Renderer m_MyRenderer = null;

    private void Awake()
    {
        m_MyRenderer = this.GetComponent<Renderer>();
        m_MyRenderer.material = m_AllMat[m_CurLevelIndex];
    }

    public void SetUpdateMat(int SetCurLevelIndex)
    {
        if (0 > SetCurLevelIndex || SetCurLevelIndex >= m_AllMat.Length)
            return;

        m_MyRenderer.material = m_AllMat[SetCurLevelIndex];
        m_CurLevelIndex = SetCurLevelIndex;
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
