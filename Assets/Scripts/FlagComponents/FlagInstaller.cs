using System;
using System.Collections;
using UnityEngine;

public class FlagInstaller : MonoBehaviour
{
    public event Action IsInstalled;
    public event Action<Vector3> GetNewPoint;

    public void MonitorPointOfSticking(Flag flag)
    {
        StartCoroutine(Monitor(flag));
    }

    private IEnumerator Monitor(Flag flag)
    {
        while (true)
        {
            ApplyPoinOfSticking(flag);

            yield return null;
        }
    }

    private void ApplyPoinOfSticking(Flag flag)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent(out Map field))
                {
                    flag.NewBasePointIsGot -= field.SendGoordinatesToBaseSpawner;

                    if (flag.IsTakenFromBase == true && flag.IsTakenFromField == false)
                    {
                        flag.SetCoordinates(hit.point);

                        flag.TurnOffTransporter();

                        flag.SetStatusTakenFromField();

                        IsInstalled?.Invoke();

                        flag.NewBasePointIsGot += field.SendGoordinatesToBaseSpawner;
                    }               
                }
            }
        }
    }
}