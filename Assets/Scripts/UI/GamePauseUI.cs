using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainmenuButton;

    private void Awake()
    {
       resumeButton.onClick.AddListener(() =>
       {
           SuperTanksGameManager.Instance.TogglePauseGame();
       });
       mainmenuButton.onClick.AddListener(() =>
       {
           Loader.Load(Loader.Scene.MainMenuScene);
       });
    }
    private void Start()
    {
        SuperTanksGameManager.Instance.OnGamePaused += SuperTanksGameManager_OnGamePaused;
        SuperTanksGameManager.Instance.OnGameUnpaused += SuperTanksGameManager_OnGameUnpaused;
        Hide();
    }

    private void SuperTanksGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void SuperTanksGameManager_OnGamePaused(object sender, EventArgs e)
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
