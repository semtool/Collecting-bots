using System;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    private Base _home;

    public event Action<int> IsCounted;
    public event Action<int> IsCorrected;
    public event Action IsPut;

    public int BallCounter { get; private set; }

    private void Awake()
    {
        _home = GetComponent<Base>();

        BallCounter = 0;
    }

    public void CorrectRealNumberBallsInStorage(int numberOfBalls)
    {
        BallCounter -= numberOfBalls;

        IsCorrected?.Invoke(BallCounter);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball item))
        {
            BallCounter++;

            IsCounted?.Invoke(BallCounter);

            IsPut?.Invoke();

            _home.BallSpawner.PutBallToPool(item);
        }
    }
}