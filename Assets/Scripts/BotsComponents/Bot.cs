using UnityEngine;

[RequireComponent(typeof(Router))]
[RequireComponent(typeof(ColorChanger))]
public class Bot : MonoBehaviour
{
    private ColorChanger _colorChanger;

    public bool IsBuilder { get; private set; }

    public bool IsBusy { get; private set; }

    public Router BotRouter { get; private set; }

    private void Awake()
    {
        BotRouter = GetComponent<Router>();

        _colorChanger = GetComponent<ColorChanger>();
    }

    private void Start()
    {
        MakeNotBusy();
    }

    private void OnEnable()
    {
        BotRouter.IsFree += MakeNotBusy;
    }

    private void OnDisable()
    {
        BotRouter.IsFree -= MakeNotBusy;
    }

    public void MakeBusy()
    {
        IsBusy = true;

        _colorChanger.SetColor(Color.black);
    }

    public void MakeBuilder()
    {
        IsBuilder = true;
    }

    public void MakeNotBuilder()
    {
        IsBuilder = false;
    }

    public void MakeNotBusy()
    {
        IsBusy = false;

        _colorChanger.SetColor(Color.green);
    }
}