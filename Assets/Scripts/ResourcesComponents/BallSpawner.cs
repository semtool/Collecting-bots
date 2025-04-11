using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private BallPool _pool;
    [SerializeField] private float _minOutsideNumber;
    [SerializeField] private float _maxOutsideNumber;
    [SerializeField] private float _minInsideNumber;
    [SerializeField] private float _maxInsideNumber;
    [SerializeField] private float _spawnInterval;

    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_spawnInterval);
    }

    private void Start()
    {
        StartCoroutine(CreateBall());
    }

    private IEnumerator CreateBall()
    {
        while (enabled)
        {
            yield return _wait;

            Ball ball = _pool.GetObjectFromPool();

            ball.SetFreeStatus();

            SetCoordinates(ball);
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

    public void PutBallToPool(Ball item)
    {
        //IsPutInStorage?.Invoke();

        _pool.PutObjectToPool(item);
    }
}