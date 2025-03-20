using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseStorage))]
[RequireComponent(typeof(BaseRadar))]
public class Base : MonoBehaviour
{
    [SerializeField] private BallSpawner _spawner;
    [SerializeField] private BaseStorage _baseStorage;

    private List<Ball> balls;
    private BaseRadar _radar;

    public event Action<Collider> IsGet;

    private void Awake()
    {
        _baseStorage = GetComponent<BaseStorage>();

        _radar = GetComponent<BaseRadar>();

        balls = new List<Ball>();
    }

    private void OnEnable()
    {
        _radar.AreDetected += ChooseBot;
    }

    private void ChooseBot(Collider[] bots, Collider[] balls)
    {
        foreach (var init in bots)
        {
            if (init.TryGetComponent(out Bot bot))
            {
                if (bot.IsBusy == false)
                {
                    Ball ball = GetOneCoin(balls);

                    if (ball != null)
                    {
                        ball.SetStatusOfGoal();

                        bot.MakeBusy();

                        bot.BotRouter.MoveOnWay(ball, _baseStorage.transform.position);
                    }
                }
            }
        }
    }

    private Ball GetOneCoin(Collider[] colliders)
    {
        Ball ball = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            balls.Add(colliders[i].GetComponent<Ball>());
        }

        ball = balls.Find(item => item.IsUnderControl == false);

        return ball;
    }

    private void OnDisable()
    {
        _radar.AreDetected -= ChooseBot;
    }
}