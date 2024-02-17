using DG.Tweening;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private float _timerToDestroy = 2f;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private bool _isStarPickedUp = false;

    private Vector3 _startScale;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        _startScale = transform.localScale;

        OnStarSpawned();
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (_isStarPickedUp)
            return;

        if (_timerToDestroy <= 0)
        {
            DestroyMe();
            _timerToDestroy = 4f;
        }
        else
            _timerToDestroy -= Time.fixedDeltaTime;
    }
    private void OnStarSpawned()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(_startScale, 0.3f).SetLink(gameObject);

        _spriteRenderer.color = new Color32(255, 255, 255, 0);
        _spriteRenderer.DOFade(1, 0.3f).SetLink(gameObject);
    }
    private void DestroyMe()
    {
        _spriteRenderer.DOFade(0, 0.5f).SetLink(gameObject);
        transform.DOScale(Vector3.zero, 0.5f).SetLink(gameObject);
        _collider.enabled = false;
        Destroy(gameObject, 2.1f);
    }
    public void OnStarPickedUp()
    {
        if (_isStarPickedUp)
            return;

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.StarPickedUp, 1f, 1f);
        DestroyMe();
        _isStarPickedUp = true;
        PlayerScore.Instance.AddScore();
        Destroy(gameObject, 2.1f);
    }
}
