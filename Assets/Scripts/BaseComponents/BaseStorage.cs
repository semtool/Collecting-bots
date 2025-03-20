using System;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    public event Action<Ball> IsTouched;

    public int Counter { get; private set; }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball item))
        {
            IsTouched?.Invoke(item);
        }
    }
}