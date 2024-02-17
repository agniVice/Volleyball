using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;

    [SerializeField] [Range(10f, 50f)] private float _minStartPower;
    [SerializeField] [Range(50f, 100f)] private float _maxStartPower;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    private Transform _target;

    private BallSpawner _spawner;

    private bool _isCathed = false;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _sprites[PlayerPrefs.GetInt("LastSelected", 0)];
    }
    public void Initialize(Transform target, BallSpawner spawner)
    {
        _spawner = spawner;
        _target = target;
        _rigidBody.AddForce(GetRandomVelocity());
    }
    private Vector2 GetRandomVelocity()
    {
        return _target.position - transform.position * Random.Range(_minStartPower, _maxStartPower);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (collision.gameObject.CompareTag("Finish"))
        {
            GameState.Instance.FinishGame();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (collision.CompareTag("Star"))
        {
            if (!_isCathed)
                return;

            collision.GetComponent<Star>().OnStarPickedUp();
        }
    }
    public void OnBallCathed(Transform hands)
    {
        if (_isCathed)
            return;

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.BallCathed, 1f, 1f);
        _spawner.OnBallCatched(this);
        _spriteRenderer.DOFade(0, 3f).SetLink(gameObject);
        _isCathed = true;
        Invoke("DestroyBall", 3f);
    }
    private void DestroyBall()
    {
        Destroy(gameObject);
    }
}
