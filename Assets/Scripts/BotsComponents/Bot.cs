using UnityEngine;

[RequireComponent(typeof(Router))]
[RequireComponent(typeof(ColorChanger))]
public class Bot : MonoBehaviour
{
    [SerializeField] private BaseStorage _base;

    private ColorChanger _colorChanger;

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

    public void MakeBusy()
    {
        IsBusy = true;

        _colorChanger.SetColor(Color.black);
    }

    private void MakeNotBusy()
    {
        IsBusy = false;

        _colorChanger.SetColor(Color.green);
    }

    private void OnDisable()
    {
        BotRouter.IsFree -= MakeNotBusy;
    }
}