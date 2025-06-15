using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseRadar))]
[RequireComponent(typeof(BaseStorage))]
[RequireComponent(typeof(FlagTaker))]
[RequireComponent(typeof(BotSpawner))]
[RequireComponent(typeof(ResourceUnitDetector))]
public class Base : MonoBehaviour
{
    [SerializeField] private Flag _ownFlag;

    private List<Bot> _bots = new List<Bot>();
    private int _minNumberBots = 1;
    private GlobalResourceStorage _gobalStorage;
    private FlagTaker _flagTaker;
    private BaseRadar _radar;
    private BotSpawner _botSpawner;
    private BaseStorage _storage;
    private ResourceUnitDetector _unitsDetector;
    private int _numberBallsToPrepareBuilderBot = 5;
    private int _numberBallsToPrepareUsualBot = 3;

    public bool FlagIsInstaled { get; private set; }

    public Bot OnlyBuilderBot { get; private set; }

    public bool FlagIsTeken { get; private set; }

    public BallSpawner BallSpawner { get; private set; }

    public Flag Flag { get; private set; }


    public bool BaseIsBuisHitRay = false;

    private void Awake()
    {
        OnlyBuilderBot = null;

        _radar = GetComponent<BaseRadar>();

        _flagTaker = GetComponent<FlagTaker>();

        _botSpawner = GetComponent<BotSpawner>();

        _storage = GetComponent<BaseStorage>();

        _unitsDetector = GetComponent<ResourceUnitDetector>();

        Flag = _ownFlag;
    }

    private void Start()
    {
        _ownFlag.TurnOnChildObject(gameObject.transform);

        FlagIsInstaled = false;

        _ownFlag.SetStatusNotTakenFromBase();

        var position = Vector3.up.normalized;
    }

    private void OnEnable()
    {
        _radar.AreDetected += AccumulateBallsInBaseStorage;

        _unitsDetector.IsReceived += UseResourcesFromStorage;

        _botSpawner.IsCreated += PutNewBotToBotList;

        _ownFlag.IsReturnedOnParentBase += SetToParentBaseUsualOperatingMode;
    }

    private void OnDisable()
    {
        _radar.AreDetected -= AccumulateBallsInBaseStorage;

        _unitsDetector.IsReceived -= UseResourcesFromStorage;

        _botSpawner.IsCreated -= PutNewBotToBotList;

        _ownFlag.IsReturnedOnParentBase -= SetToParentBaseUsualOperatingMode;
    }

    public void SetFlagIsNotTakenFromBase()
    {
        FlagIsTeken = false;
    }

    public void ChangeFlagStatusReturnedToParentBase()
    {
        FlagIsInstaled = false;

        _ownFlag.SetStatusNotTakenFromBase();
    }

    public void ChangeFlagStatusInstalledForNewBase()
    {
        FlagIsInstaled = true;
    }

    public void SetPool(BallSpawner ballSpawner)
    {
        BallSpawner = ballSpawner;
    }

    public void SetGlobalStorage(GlobalResourceStorage resourceStorage)
    {
        _gobalStorage = resourceStorage;
    }

    public void CleareBuilderBotList()
    {
        OnlyBuilderBot = null;
    }

    public void CleareBotList(Bot bot)
    {
        _bots.Remove(bot);
    }

    public void RecieveFirstUsualBots(int maxNumberOfBots, Bot prefubBot)
    {
        for (int i = 0; i < maxNumberOfBots; i++)
        {
            _bots.Add(Instantiate(prefubBot));
        }
    }

    public void ReceiveBot(Bot bot)
    {
        _bots.Add(bot);
    }

    public void UseOwnFlag()
    {
        if (_ownFlag != null)
        {
            if (_ownFlag.IsTakenFromBase == false)
            {
                _flagTaker.TakeFlagFromBase(_ownFlag);

                _ownFlag.SetStatusTakenFromBase();

                _ownFlag.SetStatusNotTakenFromField();

                SetFlagIsTakenFromBase();
            }
            else
            {
                _flagTaker.PutFlagToBase(_ownFlag);

                _ownFlag.SetStatusNotTakenFromBase();

                SetFlagIsNotTakenFromBase();
            }
        }
    }

    private void AccumulateBallsInBaseStorage(Collider[] ballColliders)
    {
        foreach (var collider in ballColliders)
        {
            Ball ball = collider.GetComponent<Ball>();

            _gobalStorage.FillFreeBallsCollection(ball);
        }

        SendBotToBall();
    }

    private void SendBotToBall()
    {
        foreach (var unit in _bots)
        {
            if (unit.IsBusy == false)
            {
                if (TakeFreeBall(out Ball ball))
                {
                    unit.MakeBusy();

                    _gobalStorage.FillBusyBallsColleñtion(ball);

                    ball.SetStatusOfGoal();

                    unit.BotRouter.MoveOnWay(ball, this);
                }
            }
        }
    }

    private bool TakeFreeBall(out Ball ball)
    {
        ball = null;

        if (_gobalStorage.FreeBallsCollectionLength > 0)
        {
            ball = _gobalStorage.GetFirstBallFreeBallsCollection();
        }

        return ball != null;
    }

    private void SetFlagIsTakenFromBase()
    {
        FlagIsTeken = true;
    }

    private void UseResourcesFromStorage(Ball ball)
    {
        _storage.IncreaseNumberOfResourses();

        BallSpawner.PutBallToPool(ball);

        if (FlagIsInstaled && _bots.Count > _minNumberBots && FlagIsTeken)
        {
            AppointBuilderBot();
        }
        else
        {
            CreateNewUsualBot(_storage.BallCounter);
        }
    }

    private void PutNewBotToBotList(Bot bot)
    {
        _bots.Add(bot);

        SetStartPosition(bot);
    }

    private void CreateNewUsualBot(int numberOfBalls)
    {
        if (numberOfBalls >= _numberBallsToPrepareUsualBot)
        {
            LaunchBot();
        }
    }

    private void LaunchBot()
    {
        _storage.CorrectRealNumberBallsInStorage(_numberBallsToPrepareUsualBot);

        _botSpawner.CreateNewBot();
    }

    private void SetStartPosition(Bot bot)
    {
        bot.transform.position = transform.position;
    }

    private void AppointBuilderBot()
    {
        if (_storage.BallCounter >= _numberBallsToPrepareBuilderBot)
        {
            foreach (var unit in _bots)
            {
                if (OnlyBuilderBot == null)
                {
                    if (unit.IsBusy == false && unit.IsBuilder == false)
                    {
                        unit.MakeBusy();

                        unit.MakeBuilder();

                        OnlyBuilderBot = unit;

                        _storage.CorrectRealNumberBallsInStorage(_numberBallsToPrepareBuilderBot);

                        LaunchToOwnBaseFlag();
                    }
                }
            }
        }
    }

    private void LaunchToOwnBaseFlag()
    {
        OnlyBuilderBot.BotRouter.MoveToNewBasePlace(_ownFlag);
    }

    private void SetToParentBaseUsualOperatingMode()
    {
        ChangeFlagStatusReturnedToParentBase();

        if (OnlyBuilderBot != null)
        {
            OnlyBuilderBot.BotRouter.StopMoveToBuildNewBase();

            OnlyBuilderBot.MakeNotBusy();

            OnlyBuilderBot = null;
        }

        CreateNewUsualBot(_storage.BallCounter);
    }
}