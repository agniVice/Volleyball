using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _starPositions;
    [SerializeField] private GameObject _starPrefab;

    [SerializeField] private float _minSpawnTime = 1f;
    [SerializeField] private float _maxSpawnTime = 3f;

    private float _timer;

    private void Awake()
    {

    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (_timer > 0)
            _timer -= Time.fixedDeltaTime;
        else
        {
            SpawnRandomStar();
            SetRandomTimer();
        }
    }
    private void SpawnRandomStar()
    {
        var star = Instantiate(_starPrefab);
        star.transform.position = _starPositions[Random.Range(0, _starPositions.Count)].position;
    }
    private void SetRandomTimer()
    {
        _timer = Random.Range(_minSpawnTime, _maxSpawnTime);
    }
}
