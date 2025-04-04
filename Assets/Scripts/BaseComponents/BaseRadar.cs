using System;
using System.Collections;
using UnityEngine;

public class BaseRadar : MonoBehaviour
{
    [SerializeField] private LayerMask _ballLayerMask;
    [SerializeField] public float _maxMonitoringRudius;

    private float _intervalOfMonitoring = 1f;
    private WaitForSeconds _wait;

    public event Action<Collider[]> AreDetected;

    private void Awake()
    {
        _wait = new WaitForSeconds(_intervalOfMonitoring);
    }

    private void Start()
    {
        StartCoroutine(ScanSpace());
    }

    private IEnumerator ScanSpace()
    {
        while (enabled)
        {
            yield return _wait;

            Monitor();
        }
    }

    private void Monitor()
    {
        AreDetected?.Invoke(ToDetectBalls());
    }

    private Collider[] ToDetectBalls()
    {
        Collider[] ballColliders = Physics.OverlapSphere(transform.position, _maxMonitoringRudius, _ballLayerMask);

        return ballColliders;
    }
}