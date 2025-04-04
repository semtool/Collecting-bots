using UnityEngine;

public class FlagSelfDetector : MonoBehaviour
{
    private void OnMouseDown()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out Flag flag))
            {
                flag.SetStatusNotTakenFromField();

                flag.TurnOnTransporter();
            }
        }
    }
}