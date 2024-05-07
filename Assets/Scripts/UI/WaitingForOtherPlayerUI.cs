using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayerUI : MonoBehaviour
{
    private void Start()
    {
        SuperTanksGameManager.Instance.OnLocalPlayerReadyChanged += SuperTanksGameManager_OnLocalPlayerReadyChanged;
        SuperTanksGameManager.Instance.OnStageChanged += SuperTanksGameManager_OnStageChanged;
        Hide();
    }
    private void SuperTanksGameManager_OnStageChanged(object sender, EventArgs e)
    {
        if(SuperTanksGameManager.Instance.isCountdownToStartActive())
        {
            Hide();
        }
        
    }
    private void SuperTanksGameManager_OnLocalPlayerReadyChanged(object sender, EventArgs e)
    {
        if(SuperTanksGameManager.Instance.IsLocalPlayerReady())
        {
             Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
