using UnityEngine;

public class GameTimer : MonoBehaviour, ISubscriber
{
    public static GameTimer Instance;

    public float DefaultTimer;
    public float Timer;

    [SerializeField] private float _extraTime = 3f;

    private bool _isEnabled;

    private void Awake()
    {
        if(Instance != null && Instance != this) 
            Destroy(gameObject); 
        else
            Instance = this;

        ResetTimer();
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (_isEnabled)
        {
            if (Timer > 0)
            {
                Timer -= Time.fixedDeltaTime;
            }
            else
            {
                _isEnabled = false;
                GameState.Instance.FinishGame();
            }
        }
    }
    public void ResetTimer()
    {
        _isEnabled = false;
        Timer = DefaultTimer;
    }
    public void StartTimer()
    {
        _isEnabled  = true;
    }
    public void AddExtraTime()
    {
        //Timer += _extraTime;
        Timer = DefaultTimer;
    }
    public void SubscribeAll()
    {
        GameState.Instance.ScoreAdded += AddExtraTime;
    }

    public void UnsubscribeAll()
    {
        GameState.Instance.ScoreAdded -= AddExtraTime;
    }
}
