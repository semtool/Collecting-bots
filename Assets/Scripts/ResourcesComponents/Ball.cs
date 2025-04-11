using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ColorChanger))]
public class Ball : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Collider _collider;
    private ColorChanger _colorChanger;
    private float _xCoordinateRelativeParent = 0f;
    private float _yCoordinateRelativeParent = 1f;
    private float _zCoordinateRelativeParent = 1f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _collider = GetComponent<Collider>();

        _colorChanger = GetComponent<ColorChanger>();
    }

    public void MakeBusy(Transform botTransform)
    {
        SetColorStatusOfDelivering();

        CreateChildObject(botTransform);

        _collider.enabled = false;

        _rigidbody.isKinematic = true;

        transform.localPosition = new Vector3(_xCoordinateRelativeParent, _yCoordinateRelativeParent, _zCoordinateRelativeParent);
    }


    public void MakeNotBusy()
    {
        CanscelChildObject();

        _collider.enabled = true;

        _colorChanger.SetColor(Color.white);

        _rigidbody.isKinematic = false;
    }

    public void SetStatusOfGoal()
    {
        _colorChanger.SetColor(Color.red);
    }

    public void SetFreeStatus()
    {
        _colorChanger.SetColor(Color.white);
    }

    private void SetColorStatusOfDelivering()
    {
        _colorChanger.SetColor(Color.green);
    }

    private void CreateChildObject(Transform botTransform)
    {
        gameObject.transform.SetParent(botTransform);
    }

    private void CanscelChildObject()
    {
        gameObject.transform.SetParent(null);
    }
}