using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseRadar))]
[RequireComponent(typeof(BaseStorage))]
[RequireComponent(typeof(GlobalResourceStorage))]
public class Base : MonoBehaviour
{
    [SerializeField] private BallSpawner _spawner;
    [SerializeField] private List<Bot> _botList = new();

    private BaseStorage _baseStorage;
    private GlobalResourceStorage _globalStorage;
    private BaseRadar _radar;
    private WaitForSeconds _wait;
    private float _interval = 1f;

    private void Awake()
    {
        _radar = GetComponent<BaseRadar>();

        _baseStorage = GetComponent<BaseStorage>();

        _globalStorage = GetComponent<GlobalResourceStorage>();

        _wait = new WaitForSeconds(_interval);
    }

    private void Start()
    {
        StartCoroutine(SendBotsToBalls());
    }

    private void OnEnable()
    {
        _radar.AreDetected += SendBallsToGlobalStorage;

        _baseStorage.IsTouched += _globalStorage.DeleteItemFromBusyCollection;
    }

    private IEnumerator SendBotsToBalls()
    {
        while (enabled)
        {
            SendBotToBall();

            yield return _wait;
        }
    }

    private void SendBotToBall()
    {
        foreach (var unit in _botList)
        {
            if (unit.IsBusy == false)
            {
                Ball ball = TakeFreeBall();

                if (ball != null)
                {
                    ball.SetStatusOfGoal();

                    unit.MakeBusy();

                    _globalStorage.FillBusyBallsColleñtion(ball);

                    unit.BotRouter.MoveOnWay(ball, _baseStorage.transform.position);
                }
            }
        }
    }

    private void SendBallsToGlobalStorage(Collider[] ballColliders)
    {
        foreach (var ball in ballColliders)
        {
            _globalStorage.FillFreeBallsCollection(ball);
        }
    }

    private Ball TakeFreeBall()
    {
        Ball ball = null;

        if (_globalStorage.FreeBalls.Count > 0)
        {
            ball = _globalStorage.GetFirstBallFreeBallsCollection();
        }

        return ball;
    }

    private void OnDisable()
    {
        _radar.AreDetected -= SendBallsToGlobalStorage;

        _baseStorage.IsTouched -= _globalStorage.DeleteItemFromBusyCollection;
    }
}