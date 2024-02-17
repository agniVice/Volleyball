using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallSpawner : MonoBehaviour, ISubscriber
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<Transform> _targets;
    [SerializeField] private GameObject _ballPrefab;

    private List<GameObject> _balls = new List<GameObject>();

    [SerializeField] [Range(0f, 3f)] private float _minDelay = 0f;
    [SerializeField] [Range(0f, 3f)] private float _maxDelay = 2f;

    public void SubscribeAll()
    {
        GameState.Instance.GameStarted += SpawnBallWithRandomDelay;
        GameState.Instance.BallCathed += SpawnBallWithRandomDelay;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameStarted -= SpawnBallWithRandomDelay;
        GameState.Instance.BallCathed -= SpawnBallWithRandomDelay;
    }
    private void SpawnBallWithRandomDelay()
    {
        Invoke("SpawnBall", Random.Range(_minDelay, _maxDelay));
    }
    private void SpawnBall()
    {
        var ball = Instantiate(_ballPrefab);
        ball.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
        _balls.Add(ball);

        ball.GetComponent<Ball>().Initialize(_targets[Random.Range(0, _targets.Count)], this);

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.BallSpawned, 1f, 1f);
    }
    public void OnBallCatched(Ball ball)
    {
        _balls.Remove(ball.gameObject);
    }
}
