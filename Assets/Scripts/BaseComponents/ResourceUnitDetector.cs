using System;
using UnityEngine;

public class ResourceUnitDetector : MonoBehaviour
{
    public event Action<Ball> IsReceived;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball item))
        {
            IsReceived.Invoke(item);
        }
    }
}