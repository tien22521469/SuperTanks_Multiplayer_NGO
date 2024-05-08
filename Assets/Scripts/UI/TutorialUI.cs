using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public void Start()
    {
        GameInput.Instance.GetMovementAction+=
    }
    private void Show()
    {
        gameObject.SetActive(false);
    }

    private void Hide()
    {
        gameObject.SetActive(true);
    }
}
