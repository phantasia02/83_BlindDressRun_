﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CChangeScenes
{
    

    public void ChangeScenes(string lScenesName)
    {
        //SceneManager.LoadScene(lScenesName);

        //GlobalData.g_CurSceneName = lScenesName;

        //string[] sArray = lScenesName.Split(new string[] { GlobalData.g_scLevelPrefix }, StringSplitOptions.RemoveEmptyEntries);

        //if (sArray.Length == 1)
        //    GlobalData.g_LevelIndex = int.Parse(sArray[0]);
    }

    public void LoadGameScenes()
    {
        //if (lpLevelIndex < 0 || lpLevelIndex >= GlobalData.SharedInstance.LevelGameObj.Length)
        //    return;

        GlobalData.g_CurSceneName = GlobalData.g_GameScenesName;
        //CSaveManager.m_status.m_LevelIndex = lpLevelIndex;
        SceneManager.LoadScene(GlobalData.g_CurSceneName);
    }

    public void LoadTestScenes()
    {
        GlobalData.g_CurSceneName = GlobalData.g_testScenesName;
        SceneManager.LoadScene(GlobalData.g_CurSceneName);
    }

    public void SetNextLevel()
    {
        int lTempNextLevelIndex = CSaveManager.m_status.m_LevelIndex;
        lTempNextLevelIndex++;

        if (lTempNextLevelIndex >= GlobalData.SharedInstance.LevelGameObj.Length)
            lTempNextLevelIndex = 0;

        CSaveManager.m_status.m_LevelIndex = lTempNextLevelIndex;
        CSaveManager.SharedInstance.Save();
    }


    public void ResetScene()
    {
        SceneManager.LoadScene(GlobalData.g_CurSceneName);
    }

    //public int NameToIndex(string lpScenesName)
    //{
    //    for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
    //    {
    //        if ( SceneManager.GetSceneByBuildIndex(i).name == lpScenesName)
    //            return i;
    //    }

    //    return -1;
    //}
}
