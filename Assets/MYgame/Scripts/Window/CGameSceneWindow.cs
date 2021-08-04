using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CGameSceneWindow : CSingletonMonoBehaviour<CGameSceneWindow>
{

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    [SerializeField] GameObject m_ShowObj       = null;
    [SerializeField] Button m_ResetButton       = null;
    //[SerializeField] Text m_FloorEndText        = null;
    //[SerializeField] Text m_FloorEndTextShadow  = null;
    [SerializeField] TextMeshProUGUI m_FuelText            = null;
    [SerializeField] TextMeshProUGUI m_FuelTextShadow      = null;

    [SerializeField] Image m_Temptext = null;

    public void SetTemptextPos(Vector3 pos)
    {
        m_Temptext.transform.position = pos;
    }

    private void Awake()
    {
        m_ResetButton.onClick.AddListener(() => {
            m_ChangeScenes.ResetScene();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetShow() { return m_ShowObj.activeSelf; }
    public void ShowObj(bool showObj)
    {
        if (showObj)
        {

        }
        m_ShowObj.SetActive(showObj);
        // CGameSceneWindow.SharedInstance.SetCurState(CGameSceneWindow.EState.eEndStop);
    }
}
