using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Router : MonoBehaviour
{
    private Bot _bot;
    private Mover _botMover;
    private Coroutine _coroutineForMovingToGoal;
    private Coroutine _coroutineForMovingToBase;
    private Coroutine _coroutineForMovingToFlag;
    private float _distanceToGoal = 1f;
    private float _distanceToStorage = 3f;

    public event Action IsFree;

    private void Awake()
    {
        _bot = GetComponent<Bot>();

        _botMover = GetComponent<Mover>();
    }

    public void MoveOnWay(Ball goal, Base homeBase)
    {
        PrepareCoroutine(_coroutineForMovingToGoal);

        _coroutineForMovingToGoal = StartCoroutine(MoveToGoal(goal, homeBase));
    }

    public void MoveToNewBasePlace(Flag flag)
    {
        PrepareCoroutine(_coroutineForMovingToFlag);

        _coroutineForMovingToFlag = StartCoroutine(MoveToFlag(flag));
    }

    private IEnumerator MoveToFlag(Flag flag)
    {
        _botMover.SetBusySpeed();

        while (transform.position != flag.transform.position)
        {
            _botMover.Move(flag.transform.position);

            yield return null;
        }

        flag.SendNewBasePoint();

        flag.SetStartCoordinates();

        flag.InformBase();

        _bot.MakeNotBusy();
    }

    private IEnumerator MoveToGoal(Ball goal, Base storage)
    {
        _botMover.SetNotBusySpeed();

        while (GetDistance(goal.transform.position) > _distanceToGoal)
        {
            _botMover.Move(goal.transform.position);

            yield return null;
        }

        goal.MakeBusy(gameObject.transform);

        PrepareCoroutine(_coroutineForMovingToBase);

        _coroutineForMovingToBase = StartCoroutine(MoveToBase(storage, goal));
    }

    private IEnumerator MoveToBase(Base storage, Ball goal)
    {
        _botMover.SetBusySpeed();

        while (GetDistance(storage.transform.position) > _distanceToStorage)
        {
            _botMover.Move(storage.transform.position);

            yield return null;
        }

        IsFree?.Invoke();

        goal.MakeNotBusy();
    }

    private void PrepareCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private float GetDistance(Vector3 goal)
    {
        return Vector3.Distance(transform.position, goal);
    }
}