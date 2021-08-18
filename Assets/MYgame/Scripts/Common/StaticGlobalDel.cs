using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticGlobalDel 
{
    public enum EMovableState
    {
        eNull           = 0,
        eWait           = 1,
        eMove           = 2,
        eHit            = 3,
        eWin            = 4,
        eOver           = 5,
        eMax
    }

    public enum ELayerIndex
    {
        eWinFloor       = 4,
        eFloor          = 8,
        eNotWalkable    = 9,
        eMovable        = 10,
        eMax
    }

    // tag Door
    // tag Lipstick
    // tag Mud
    // tag End

    public const int    g_WinLayerMask                  = 8;
    public const int    g_FloorLayerMask                = 256;
    public const int    g_NotWalkableLayerMask          = 512;
    public const int    g_MovableLayerMask              = 1024;
    public const float  g_fcbaseWidth                   = 1242.0f;
    public const float  g_fcbaseHeight                  = 2688.0f;
    public const float  g_fcbaseOrthographicSize        = 18.75f;
    public const float  g_fcbaseResolutionWHRatio       = g_fcbaseWidth / g_fcbaseHeight;
    public const float  g_fcbaseResolutionHWRatio       = g_fcbaseHeight / g_fcbaseWidth;
    public const float  g_TUA                           = Mathf.PI * 2.0f;
    // ============= Speed ====================
    public const float g_DefMovableTotleSpeed = 15.0f;
    // ============= Hp ====================
    public const int g_DefHp = 10;
    public const int g_MaxHp = 20;
    public const int g_RefFXGoodHp = 10;
    public const int g_RefFXBadHp  = 10;
    public const float g_DefHpRatio = 0.5f;

    public static GameObject NewFxAddParentShow(this Transform ParentTransform, CGGameSceneData.EAllFXType Fxtype)
    {
        CGGameSceneData lTempGGameSceneData = CGGameSceneData.SharedInstance;
        GameObject lTempFx = GameObject.Instantiate(lTempGGameSceneData.m_AllFX[(int)Fxtype], ParentTransform);
        lTempFx.transform.position = ParentTransform.position;

        return lTempFx;
    }
}
