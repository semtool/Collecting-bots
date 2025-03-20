using UnityEngine;

[RequireComponent(typeof(Router))]
public class Bot : MonoBehaviour
{
    [SerializeField] private BaseStorage _base;

    private Renderer _renderer;

    public bool IsBusy { get; private set; }

    public Router BotRouter { get; private set; }

    private void Awake()
    {
        BotRouter = GetComponent<Router>();

        _renderer = gameObject.GetComponent<Renderer>();
    }

    private void Start()
    {
        IsBusy = false;

        SetColor();
    }


    private void OnEnable()
    {
        BotRouter.IsFree += MakeNotBusy;
    }

    private void SetColor()
    {
        _renderer.material.color = Color.black;
    }

    public void MakeBusy()
    {
        IsBusy = true;
    }

    public void MakeNotBusy()
    {
        IsBusy = false;
    }

    private void OnDisable()
    {
        BotRouter.IsFree -= MakeNotBusy;
    }
}