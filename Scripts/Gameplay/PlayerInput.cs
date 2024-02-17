using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour, IInitializable, ISubscriber
{ 
    public static PlayerInput Instance;

    public Action PlayerMouseDownLeft;
    public Action PlayerMouseUpLeft;

    public Action PlayerMouseDownRight;
    public Action PlayerMouseUpRight;

    public bool IsEnabled;// { get; private set; }

    private bool _isInitialized;

    private bool _firstTouch = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void OnEnable()
    {
        if (!_isInitialized)
            return;

        SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    public void Initialize()
    {
        SubscribeAll();
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameStarted += EnableInput;
        GameState.Instance.GameUnpaused += EnableInput;
        GameState.Instance.GamePaused += DisableInput;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameStarted -= EnableInput;
        GameState.Instance.GameUnpaused -= EnableInput;
        GameState.Instance.GamePaused -= DisableInput;
    }
    public void EnableInput()
    { 
        IsEnabled= true;
    }
    public void DisableInput() 
    {
        IsEnabled = false;
    }
    public void OnPlayerMouseDown()
    {
        if (!_firstTouch)
        {
            _firstTouch = true;
            GameState.Instance.StartGame();
        }

        if (!IsEnabled)
            return;

        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;
    }
    public void OnPlayerMouseUp(int side) 
    {
        if (!IsEnabled)
            return;

        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (side == 0)
            PlayerMouseUpLeft?.Invoke();
        else
            PlayerMouseUpRight?.Invoke();
    }
    private void OnMouseDown()
    {
        OnPlayerMouseDown();
    }
}