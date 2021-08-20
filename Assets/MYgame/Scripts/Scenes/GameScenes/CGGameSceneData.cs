using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CGGameSceneData : CSingletonMonoBehaviour<CGGameSceneData>
{

    public enum EDoorType
    {
        eNormal     = 0,
        eGood       = 1,
        eBad        = 2,
        eMax
    };

    public enum EPlayAccessoriesType
    {
        eClothing   = 0,        // 衣服
       // ePants      = 1,        // 褲子
        eShoe       = 1,        // 鞋子
        eMax
    };

    public enum EAllFXType
    {
        eFlareLipstick  = 0,
        eFlareGoodDoor  = 1,
        eUgly           = 2,
        eEnd            = 3,
        eUglyTemp       = 4,
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
