using System.Collections.Generic;
using UnityEngine;

public class GlobalResourceStorage : MonoBehaviour
{
    [SerializeField] private BallPool _ballPool;

    private Queue<Ball> _freeBalls;
    private List<Ball> _busyBalls;

    public int FreeBallsCollectionLength => _freeBalls.Count;

    private void Awake()
    {
        _freeBalls = new Queue<Ball>();
        _busyBalls = new List<Ball>();
    }

    private void OnEnable()
    {
        _ballPool.ObjectIsInPool += DeleteItemFromBusyCollection;
    }

    private void OnDisable()
    {
        _ballPool.ObjectIsInPool -= DeleteItemFromBusyCollection;
    }

    public void FillFreeBallsCollection(Collider collider)
    {
        if (collider.TryGetComponent(out Ball ball))
        {
            if (!_freeBalls.Contains(ball) && !_busyBalls.Contains(ball))
            {
                _freeBalls.Enqueue(ball);
            }
        }
    }

    public void FillBusyBallsColleñtion(Ball ball)
    {
        if (!_busyBalls.Contains(ball))
        {
            _busyBalls.Add(ball);
        }
    }

    public void DeleteItemFromBusyCollection(Ball ball)
    {
        _busyBalls.Remove(ball);
    }

    public Ball GetFirstBallFreeBallsCollection()
    {
        return _freeBalls.Dequeue();
    }
}