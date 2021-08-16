using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CDoor : MonoBehaviour
{
    [SerializeField] protected Renderer     m_DoorRenderer          = null;
    [SerializeField] protected Renderer     m_AccessoriesRenderer   = null;
    [SerializeField] protected GameObject   m_ShowAccessories       = null;

    [SerializeField] protected CGGameSceneData.EDoorType m_MyDoorType;
    public CGGameSceneData.EDoorType DoorType { get { return m_MyDoorType; } }
    [SerializeField] protected CGGameSceneData.EPlayAccessoriesType m_MyPlayAccessoriesType;
    public CGGameSceneData.EPlayAccessoriesType PlayAccessoriesType { get { return m_MyPlayAccessoriesType; } }



    private void Awake()
    {
        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;

        m_DoorRenderer.material         = lTempCGGameSceneData.m_AllDoorMat[(int)m_MyDoorType];
        m_AccessoriesRenderer.material  = lTempCGGameSceneData.m_AllPlayAccessoriesMat[(int)m_MyPlayAccessoriesType];
    }

    public void ShowAccessories(bool show){m_AccessoriesRenderer.gameObject.SetActive(show);}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
