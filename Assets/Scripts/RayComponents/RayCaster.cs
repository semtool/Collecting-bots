using System;
using UnityEngine;
using System.Collections;

public class RayCaster : MonoBehaviour
{
    public event Action<RaycastHit> WasHit;

    public void Start()
    {
        StartCoroutine(MonitorRayLaunch());
    }

    private IEnumerator MonitorRayLaunch()
    {
        while (true)
        {
            LaunchRay();

            yield return null;
        }
    }

    private void LaunchRay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                WasHit?.Invoke(hit);
            }
        }
    }
}