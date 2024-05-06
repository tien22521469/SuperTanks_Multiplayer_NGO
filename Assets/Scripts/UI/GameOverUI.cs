using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;
    private void Start()
    {
        SuperTanksGameManager.Instance.OnStageChanged += SuperTanksGameManager_OnStateChanged;
        Hide();
    }

    private void Update()
    {

    }
    private void SuperTanksGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (SuperTanksGameManager.Instance.isGameOver())
        {
            Show();
        }
        else
        {
            Hide();
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
