using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
 
    private void Start()
    {
        SuperTanksGameManager.Instance.OnLocalPlayerReadyChanged += SuperTanksGameManager_OnLocalPlayerReadyChanged;
        Debug.Log("TutorialUI Start");
        Show();
    }


    private void SuperTanksGameManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (SuperTanksGameManager.Instance.IsLocalPlayerReady())
        {
            Debug.Log("TutorialUI");
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

