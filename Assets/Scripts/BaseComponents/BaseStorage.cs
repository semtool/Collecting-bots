using System;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    private int _startNumberOfCounter = 0;

    public event Action<int> IsCountedForInformer;

    public event Action<int> IsCountedForIn;


    public event Action<int> IsCorrected;

    public int BallCounter { get; private set; }

    private void Awake()
    {
        BallCounter = _startNumberOfCounter;
    }

    public void CorrectRealNumberBallsInStorage(int numberOfBalls)
    {
        BallCounter -= numberOfBalls;

        IsCorrected?.Invoke(BallCounter);
    }

    public void IncreaseNumberOfResourses()
    {
        BallCounter++;

        IsCountedForInformer?.Invoke(BallCounter);
    }
}