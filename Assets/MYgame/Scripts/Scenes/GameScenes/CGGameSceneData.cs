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

    public enum EAllFXType
    {
        eFlareLipstick  = 0,
        eFlareGoodDoor  = 1,
        eUgly           = 2,
        eEnd            = 3,
        eMax,
    };

    public enum EFXEndMaterialType
    {
        eHappyGirl      = 0,
        eHappyPeople    = 1,
        eSadGirl        = 2,
        eSadPeople      = 3,
        eMax,
    };

    [SerializeField] public Material[]      m_AllDoorMat             = null;
    [SerializeField] public Material[]      m_AllPlayAccessoriesMat  = null;
    [SerializeField] public GameObject[]    m_AllFX                  = null;
    [SerializeField] public Material[]      m_AllFXEndMaterial       = null;
}
