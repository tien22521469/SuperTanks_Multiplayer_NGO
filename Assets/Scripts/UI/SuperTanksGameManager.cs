using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SuperTanksGameManager : MonoBehaviour
{
    public static SuperTanksGameManager Instance { get; private set; }

    public event EventHandler OnStageChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

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
    private float gamePlayingTimerMax = 100f;
    private bool isGamePaused;
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Awake()
    {
        Instance = this;
        state = State.WaitingForStart;
    }

    private void Update()
    {
        switch(state)
        {
            case State.WaitingForStart:
                wattingToStartTimer -= Time.deltaTime;
                if (wattingToStartTimer<0f)
                {
                    state = State.CountdownToStart;
                    OnStageChanged?.Invoke(this, EventArgs.Empty);
                }
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

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if(!isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
