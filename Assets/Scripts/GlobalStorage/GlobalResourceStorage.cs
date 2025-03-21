using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseStorage))]
public class GlobalResourceStorage : MonoBehaviour
{
    public Queue<Ball> FreeBalls { get; private set; }
    public List<Ball> BusyBalls { get; private set; }

    private void Awake()
    {
        FreeBalls = new Queue<Ball>();
        BusyBalls = new List<Ball>();
    }

    public void FillFreeBallsCollection(Collider collider)
    {
        if (collider.TryGetComponent(out Ball ball))
        {
            if (!FreeBalls.Contains(ball) && !BusyBalls.Contains(ball))
            {
                FreeBalls.Enqueue(ball);
            }
        }
    }

    public void FillBusyBallsColleñtion(Ball ball)
    {
        if (!BusyBalls.Contains(ball))
        {
            BusyBalls.Add(ball);
        }
    }

    public void DeleteItemFromBusyCollection(Ball ball)
    {
        BusyBalls.Remove(ball);
    }

    public Ball GetFirstBallFreeBallsCollection()
    {
        return FreeBalls.Dequeue();
    }
}