using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CLevelTexture
{
    public Texture m_Q_Texture = null;
    public Texture m_A_Texture = null;
}

public class CGGameSceneData : CSingletonMonoBehaviour<CGGameSceneData>
{


    public enum EDynamicallyCreateObj
    {
        eWinEffect              = 0,
        eCeilingDamageEffect    = 1,
        eCeilingDamageTag       = 2,
        eEffectHole             = 3,
        eWinCloudFloorTag       = 4,
        eMax
    };


    [SerializeField] public GameObject[] m_AllDynamicallyCreateObj = null;

    [SerializeField] public GameObject[] m_AllCloudObj  = null;
}
