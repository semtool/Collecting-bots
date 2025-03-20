using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private BaseStorage _storage;
    [SerializeField] private BallPoool _pool;
    [SerializeField] private float _minOutsideNumber;
    [SerializeField] private float _maxOutsideNumber;
    [SerializeField] private float _minInsideNumber;
    [SerializeField] private float _maxInsideNumber;

    private float _spawnInterval = 2f;

    private WaitForSeconds _wait;

    public event Action<Ball> IsReady;
    public event Action IsInStorage;

    private void Awake()
    {
        _wait = new WaitForSeconds(_spawnInterval);
    }

    private void Start()
    {
        StartCoroutine(CreateBall());
    }

    private void OnEnable()
    {
        _storage.IsTouched += PootBallToPool;
    }

    private IEnumerator CreateBall()
    {
        while (enabled)
        {
            Ball ball = _pool.GetObjectFromPool();

            ball.SetFreeStatus();

            SetCoordinates(ball);

            yield return _wait;
        }
    }

    private void SetCoordinates(Ball item)
    {
        item.transform.position = new Vector3(GetSpawnCoordinate(), 1, GetSpawnCoordinate());
    }

    private float GetSpawnCoordinate()
    {
        float number = GetMaxRandomNumber();

        while (_minInsideNumber < number && number < _maxInsideNumber)
        {
            number = GetMaxRandomNumber();
        }

        return number;
    }

    private float GetMaxRandomNumber()
    {
        return Random.Range(_minOutsideNumber, _maxOutsideNumber);
    }

    private void PootBallToPool(Ball item)
    {
        _pool.PutObjectToPool(item);

        IsInStorage?.Invoke();
    }

    private void OnDisable()
    {
        _storage.IsTouched -= PootBallToPool;
    }
}