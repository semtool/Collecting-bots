using System;
using UnityEngine;

[RequireComponent(typeof(FlagTransporter))]
[RequireComponent(typeof(FlagInstaller))]
[RequireComponent(typeof(FlagSelfDetector))]
[RequireComponent(typeof(ColorChanger))]
public class Flag : MonoBehaviour
{
    private Base _parentBase;
    private FlagTransporter _flagTransporter;
    private float _verticalOffset = 1f;

    public event Action IsReicevedNewBaseCoordinatas;
    public event Action<Vector3, Base> NewBasePointIsGot;
    public event Action IsReturnedOnParentBase;

    public bool IsTakenFromBase { get; private set; }

    public bool IsTakenFromField { get; private set; }

    public FlagInstaller FlagInstaller { get; private set; }

    public ColorChanger ColorChanger { get; private set; }

    private void Awake()
    {
        _flagTransporter = GetComponent<FlagTransporter>();

        FlagInstaller = GetComponent<FlagInstaller>();

        _parentBase = this.GetComponentInParent<Base>();

        ColorChanger = GetComponent<ColorChanger>();
    }

    private void Start()
    {
        TurnOffTransporter();

        SetStatusNotTakenFromBase();

        FlagInstaller.MonitorPointOfSticking(this);
    }

    private void OnEnable()
    {
        FlagInstaller.IsInstalled += SendSignalToLaunchBot;
    }

    private void OnDisable()
    {
        FlagInstaller.IsInstalled -= SendSignalToLaunchBot;
    }

    public void SetStatusTakenFromBase()
    {
        IsTakenFromBase = true;
    }

    public void SetStatusNotTakenFromBase()
    {
        IsTakenFromBase = false;
    }

    public void SetStatusTakenFromField()
    {
        IsTakenFromField = true;
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

    public void SetCoordinates(Vector3 vector)
    {
        Vector3 vector1 = vector;

        vector1.y = _verticalOffset;

        transform.position = vector1;
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
    }

    public void InformBase()
    {
        IsReturnedOnParentBase?.Invoke();
    }

    public void SendNewBasePoint()
    {
        NewBasePointIsGot?.Invoke(transform.position, _parentBase);
    }

    private void SendSignalToLaunchBot()
    {
        IsReicevedNewBaseCoordinatas?.Invoke();
    }
}