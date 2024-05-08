using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour
{
    private void Start()
    {
        SuperTanksGameManager.Instance.OnMultiplayerGamePaused += SuperTanksGameManager_OnMultiplayerGamePaused;
        SuperTanksGameManager.Instance.OnMultiplayerGameUnPaused += SuperTanksGameManager_OnMultiplayerGameUnPaused;
        Hide();
    }

    private void SuperTanksGameManager_OnMultiplayerGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void SuperTanksGameManager_OnMultiplayerGamePaused(object sender, EventArgs e)
    {
        Show();
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
