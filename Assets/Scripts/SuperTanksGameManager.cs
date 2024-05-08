using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SuperTanksGameManager : MonoBehaviour
{

    public static SuperTanksGameManager Instance { get; private set; }

    public event EventHandler OnStageChanged;
    private enum State
    {
        WaitingForStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    private State state;
    private float wattingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 20f;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingForStart;
    }

    private void Start()
    {
        GameInput.Instance.
    }
    private void Update()
    {
        switch(state)
        {
            case State.WaitingForStart:
                break;
            
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStageChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
            
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStageChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;            
        }
        Debug.Log(state);
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public bool isCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimer()
    {
        return 1 - (gamePlayingTimer/gamePlayingTimerMax);
    }

    public bool isGameOver()
    {
        return state == State.GameOver;
    }
}
