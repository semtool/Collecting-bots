using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Router : MonoBehaviour
{
    private Mover _botMover;
    private Coroutine _coroutineForMovingToGoal;
    private Coroutine _coroutineForMovingToBase;
    private float _distanceToGoal = 1f;
    private float _distanceToStorage = 3f;

    public event Action IsFree;

    private void Awake()
    {
        _botMover = GetComponent<Mover>();
    }

    public void MoveOnWay(Ball goal, Vector3 storage)
    {
        PrepareCoroutine(_coroutineForMovingToGoal);

        _coroutineForMovingToGoal = StartCoroutine(MoveToGoal(goal, storage));
    }

    private IEnumerator MoveToGoal(Ball goal, Vector3 storage)
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

    private IEnumerator MoveToBase(Vector3 storage, Ball goal)
    {
        _botMover.SetBusySpeed();

        while (GetDistance(storage) > _distanceToStorage)
        {
            _botMover.Move(storage);

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