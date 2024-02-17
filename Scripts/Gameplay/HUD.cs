using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Net.NetworkInformation;

public class HUD : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _splashPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _soundToggle;
    [SerializeField] private Sprite _soundEnabled;
    [SerializeField] private Sprite _soundDisabled;
    [SerializeField] private Image _timerBar;

    private bool _isInitialized;

    private void Start()
    {
        UpdateSoundImage();
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        _timerBar.fillAmount = GameTimer.Instance.Timer/ GameTimer.Instance.DefaultTimer;
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
        GetComponent<Canvas>().worldCamera = Camera.main;

        Show();

        _isInitialized = true;

        _scoreText.text = "0";

        UpdateScore();
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameStarted += HideSplash;
        GameState.Instance.GameFinished += Hide;
        GameState.Instance.GamePaused += Hide;
        GameState.Instance.GameUnpaused += Show;

        GameState.Instance.ScoreAdded += UpdateScore;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameStarted -= HideSplash;
        GameState.Instance.GameFinished -= Hide;
        GameState.Instance.GamePaused -= Hide;
        GameState.Instance.GameUnpaused -= Show;

        GameState.Instance.ScoreAdded -= UpdateScore;
    }
    private void UpdateScore()
    {
        _scoreText.transform.DOScale(1.5f, 0.3f).SetLink(_scoreText.gameObject).SetUpdate(true);
        _scoreText.transform.DOScale(1, 0.3f).SetLink(_scoreText.gameObject).SetDelay(0.3f).SetUpdate(true);

        _scoreText.text = PlayerScore.Instance.Score.ToString();
    }
    private void Show()
    {
        _panel.SetActive(true);
    }
    private void Hide()
    {
        //_panel.SetActive(false);
    }
    private void HideSplash()
    {
        _splashPanel.SetActive(false);
    }
    public void OnRestartButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnMenuButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Menu");
    }
    public void OnPauseButtonClicked()
    {
        GameState.Instance.PauseGame();
    }
    public void OnSoundButtonClicked()
    {
        AudioVibrationManager.Instance.ToggleSound();
        UpdateSoundImage();
    }
    private void UpdateSoundImage()
    {
        if (AudioVibrationManager.Instance.IsSoundEnabled)
            _soundToggle.sprite = _soundEnabled;
        else
            _soundToggle.sprite = _soundDisabled;
    }
}