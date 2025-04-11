using UnityEngine;

public class FlagTransporter : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private float _verticalOffSet = 5f;

    private void Update()
    {
        _layerMask = ~_layerMask;

        RaycastHit hit;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out Map component))
            {
                Vector3 cusorPoint = hit.point;

                cusorPoint.y = component.transform.position.y + _verticalOffSet;

                transform.position = cusorPoint;
            }
        }
    }
}