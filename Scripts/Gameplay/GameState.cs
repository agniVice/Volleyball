﻿using DG.Tweening;
using System;
using UnityEngine;

public class GameState : MonoBehaviour, IInitializable
{
    public static GameState Instance;

    public Action GameStarted;
    public Action GamePaused;
    public Action GameUnpaused;
    public Action GameFinished;
    public Action GameLosed;

    public Action BallCathed;
    public Action ScoreAdded;

    public int LevelId { get; private set; }
    public int MaxLevelCount;

    public enum State
    { 
        Ready,
        InGame,
        Paused,
        Finished
    }
    public State CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void Initialize()
    {
        //StartGame();
    }
    public void ChangeState(State state)
    {
        switch (state)
        { 
            case State.InGame:
                    {
                        break;
                    }
            case State.Paused:
                {
                    break;
                }
            case State.Finished:
                {
                    break;
                }
        }
    }
    public void StartGame()
    {
        GameStarted?.Invoke();
        CurrentState = State.InGame;
        GameTimer.Instance.StartTimer();

        LevelId = 0;

        Time.timeScale = 1.0f;
        Debug.Log("Game Started");
    }
    public void PauseGame()
    {
        GamePaused?.Invoke();
        CurrentState = State.Paused;
        Time.timeScale = 0.0f;
    }
    public void UnpauseGame()
    {
        GameUnpaused?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void FinishGame()
    {
        GameFinished?.Invoke();
        CurrentState = State.Finished;

        Camera.main.DOShakePosition(0.4f, 0.2f, fadeOut: true).SetUpdate(true);
        Camera.main.DOShakeRotation(0.4f, 0.2f, fadeOut: true).SetUpdate(true);

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Wrong, 1f, 1f);

        /*if (AudioVibrationManager.Instance.IsVibrationEnabled)
            Handheld.Vibrate();*/
    }
    public void AddScore()
    {

        if (LevelId >= MaxLevelCount)
            LevelId = 0;
        else
            LevelId += 1;

        ScoreAdded?.Invoke();
    }
}