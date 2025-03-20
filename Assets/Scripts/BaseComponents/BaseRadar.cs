using System;
using System.Collections;
using UnityEngine;

public class BaseRadar : MonoBehaviour
{
    [SerializeField] private LayerMask _coinsLayerMask;
    [SerializeField] private LayerMask _botLayerMask;

    private float _maxMonitoringRudius = 20;
    private float _intervalOfMonitoring = 1f;
    private WaitForSeconds _wait;

    public event Action<Collider[], Collider[]> AreDetected;

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
            Monitor();

            yield return _wait;
        }
    }

    private void Monitor()
    {
        AreDetected?.Invoke(ObserveToBots(), ToDetectBalls());
    }

    private Collider[] ObserveToBots()
    {
        Collider[] botsColliders = Physics.OverlapSphere(transform.position, _maxMonitoringRudius, _botLayerMask);

        return botsColliders;
    }

    private Collider[] ToDetectBalls()
    {
        Collider[] ballColliders = Physics.OverlapSphere(transform.position, _maxMonitoringRudius, _coinsLayerMask);

        return ballColliders;
    }
}