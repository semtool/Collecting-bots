using UnityEngine;
using System;

public class BaseDetector : MonoBehaviour
{
    public event Action IsTouched;
  
    private void OnMouseDown()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out Base component))
            {
                IsTouched?.Invoke();
            }
        }
    }
}