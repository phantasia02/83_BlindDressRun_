using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAnimationCallback : MonoBehaviour
{
    public delegate void ReturnAnimationCall(int Paramete);
    public ReturnAnimationCall m_KeyFramMessageCallBack = null;

    public void ReturnCall(int index)
    {
        if (m_KeyFramMessageCallBack != null)
            m_KeyFramMessageCallBack(index);
    }
}
