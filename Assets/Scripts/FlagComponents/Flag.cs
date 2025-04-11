using System;
using UnityEngine;

[RequireComponent(typeof(FlagTransporter))]
[RequireComponent(typeof(ColorChanger))]
public class Flag : MonoBehaviour
{
    private Base _parentBase;
    private FlagTransporter _flagTransporter;
    private float _verticalOffset = 1f;

    public event Action<Vector3, Base> NewBasePointIsGot;
    public event Action IsReturnedOnParentBase;

    public bool IsTakenFromBase { get; private set; }

    public bool IsTakenFromField { get; private set; }

    public ColorChanger ColorChanger { get; private set; }

    private void Awake()
    {
        _flagTransporter = GetComponent<FlagTransporter>();

        _parentBase = this.GetComponentInParent<Base>();

        ColorChanger = GetComponent<ColorChanger>();
    }

    private void Start()
    {
        TurnOffTransporter();

        SetStatusNotTakenFromBase();
    }

    public void MoveFlag()
    {
        SetStatusNotTakenFromField();

        TurnOnTransporter();
    }

    public void SetStatusTakenFromBase()
    {
        IsTakenFromBase = true;
    }

    public void SetStatusNotTakenFromBase()
    {
        IsTakenFromBase = false;
    }

    public void SetStatusNotTakenFromField()
    {
        IsTakenFromField = false;
    }

    public void TurnOnChildObject(Transform baseTransform)
    {
        gameObject.transform.SetParent(baseTransform);

        SetStartCoordinates();
    }

    public void TurnOffTransporter()
    {
        _flagTransporter.enabled = false;
    }

    public void TurnOnTransporter()
    {
        _flagTransporter.enabled = true;
    }

    public void SetStartCoordinates()
    {
        transform.localPosition = Vector3.zero;

        ColorChanger.DecreaseAlfa();

        SetStatusNotTakenFromBase();
    }

    public void InformBase()
    {
        IsReturnedOnParentBase?.Invoke();
    }

    public void SendNewBasePoint()
    {
        NewBasePointIsGot?.Invoke(transform.position, _parentBase);
    }

    public void SendSignalToLaunchBot(Map map, Vector3 vector)
    {
        _parentBase.ChangeFlagStatusInstalledForNewBase();

        if (IsTakenFromBase && !IsTakenFromField && _parentBase.FlagIsInstaled)
        {
            NewBasePointIsGot -= map.SendGoordinatesToBaseSpawner;

            TurnOffTransporter();

            SetCoordinates(vector);

            SetStatusTakenFromField();

            NewBasePointIsGot += map.SendGoordinatesToBaseSpawner;
        }
    }

    private void SetStatusTakenFromField()
    {
        IsTakenFromField = true;
    }

    private void SetCoordinates(Vector3 vector)
    {
        Vector3 vector1 = vector;

        vector1.y = _verticalOffset;

        transform.position = vector1;
    }
}