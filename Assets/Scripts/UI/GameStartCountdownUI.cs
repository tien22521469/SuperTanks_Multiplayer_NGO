using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStartCountdownUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI countdownTextUI;

    private void Start()
    {
        SuperTanksGameManager.Instance.OnStageChanged += SuperTanksGameManager_OnStateChanged;
        Hide(); 
    }

    private void Update()
    {
        countdownTextUI.text = Mathf.Ceil(SuperTanksGameManager.Instance.GetCountdownToStartTimer()).ToString();
    }
    private void SuperTanksGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (SuperTanksGameManager.Instance.isCountdownToStartActive())
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
