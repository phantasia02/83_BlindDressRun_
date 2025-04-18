﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticGlobalDel 
{
    public enum EMovableState
    {
        eNull           = 0,
        //eWait           = 1,
        //eMove           = 2,
        //eJump           = 3,
        //eCollision      = 4,
        //eWin            = 5,
        //eOver           = 6,
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

    // tag BouncingBed
    // tag CeilingDamage
    // tag Movable
    // tag NotWalkable

    public const int g_WinLayerMask         = 8;
    public const int g_FloorLayerMask       = 256;
    public const int g_NotWalkableLayerMask = 512;
    public const int g_MovableLayerMask     = 1024;
    public const float TAU = Mathf.PI * 2.0f;
}
