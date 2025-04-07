using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDetector : MonoBehaviour
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