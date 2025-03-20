using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _xCoordinateRelativeParent = 0f;
    private float _yCoordinateRelativeParent = 1f;
    private float _zCoordinateRelativeParent = 1f;

    public bool IsUnderControl { get; private set; }

    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();

        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MakeBusy(Transform botTransform)
    {
        SetColorForStatusOfBusy();

        CreateChildObject(botTransform);

        _rigidbody.isKinematic = true;

        transform.localPosition = new Vector3(_xCoordinateRelativeParent, _yCoordinateRelativeParent, _zCoordinateRelativeParent);
    }

    public void MakeNotBusy()
    {
        CanscelChildObject();

        _rigidbody.isKinematic = false;
    }

    public void SetStatusOfGoal()
    {
        IsUnderControl = true;

        SetColorForStatusOfGoal();
    }

    public void SetFreeStatus()
    {
        IsUnderControl = false;

        Renderer.material.color = Color.white;
    }

    private void SetColorForStatusOfGoal()
    {
        Renderer.material.color = Color.red;
    }

    private void SetColorForStatusOfBusy()
    {
        Renderer.material.color = Color.green;
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