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


    [SerializeField] public Material[] m_AllDoorMat             = null;
    [SerializeField] public Material[] m_AllPlayAccessoriesMat  = null;
}
