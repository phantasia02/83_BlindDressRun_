using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CGGameSceneData : CSingletonMonoBehaviour<CGGameSceneData>
{

    public enum EDoorType
    {
        eGood       = 0,
        eBad        = 1,
    };

    public enum EPlayAccessoriesType
    {
        eClothing   = 0,
        ePants      = 1,
    };

    public enum EFXType
    {
        eBeautiful  = 0,
        eUgly       = 1,
        eLove       = 2,
        eMax,
    };

    [SerializeField] public Material[]      m_AllDoorMat             = null;
    [SerializeField] public Material[]      m_AllPlayAccessoriesMat  = null;
    [SerializeField] public GameObject[]    m_AllFX                  = null;
}
