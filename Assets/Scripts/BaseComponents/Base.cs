using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[RequireComponent(typeof(BaseRadar))]
[RequireComponent(typeof(BaseStorage))]
[RequireComponent(typeof(BaseDetector))]
[RequireComponent(typeof(FlagTaker))]
[RequireComponent(typeof(BotSpawner))]
public class Base : MonoBehaviour
{
    [SerializeField] private Flag _ownFlag;

    private List<Bot> _bots = new List<Bot>();
    private List<Bot> _builderBots = new List<Bot>();
    private Coroutine _coroutineForFindingBuilderBot;
    private BaseDetector _baseSelfDetector;
    private GlobalResourceStorage _gobalStorage;
    private FlagTaker _flagTaker;
    private BaseRadar _radar;
    private BotSpawner _botSpawner;
    private BaseStorage _storage;
    private Bot _onlyBuilderBot = null;
    private WaitForSeconds _wait;
    private float _interval = 1f;
    private int _counterOfBallForNewBotCreation = 0;
    private int _startNumberOffUsedBalls = 0;
    private int _numberBallsToPrepareBuilderBot = 5;
    private int _numberBallsToPrepareUsualBot = 3;

    public ReadOnlyCollection<Bot> BuilderBots { get; private set; }
    public bool HasSentToBuildNewBase { get; private set; }
    public BallSpawner BallSpawner { get; private set; }

    private void Awake()
    {
        HasSentToBuildNewBase = false;

        _radar = GetComponent<BaseRadar>();

        _baseSelfDetector = GetComponent<BaseDetector>();

        _flagTaker = GetComponent<FlagTaker>();

        _botSpawner = GetComponent<BotSpawner>();

        _storage = GetComponent<BaseStorage>();

        _wait = new WaitForSeconds(_interval);

        BuilderBots = _builderBots.AsReadOnly();
    }

    private void Start()
    {
        StartCoroutine(SendBotsToBalls());

        _ownFlag.TurnOnChildObject(gameObject.transform);
    }

    private void OnEnable()
    {
        _radar.AreDetected += SendBallsToGlobalStorage;

        _baseSelfDetector.IsTouched += UseOwnFlag;

        _storage.IsPut += CreateNewUsualBot;

        _botSpawner.IsCreated += PutNewBotToBotList;

        _ownFlag.IsReicevedNewBaseCoordinatas += LaunchBotToBuildNewBase;

        _ownFlag.IsReturnedOnParentBase += StopSearchBuilderBot;
    }

    private void OnDisable()
    {
        _radar.AreDetected -= SendBallsToGlobalStorage;

        _baseSelfDetector.IsTouched -= UseOwnFlag;

        _storage.IsPut -= CreateNewUsualBot;

        _botSpawner.IsCreated -= PutNewBotToBotList;

        _ownFlag.IsReicevedNewBaseCoordinatas -= LaunchBotToBuildNewBase;

        _ownFlag.IsReturnedOnParentBase -= StopSearchBuilderBot;
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
        _builderBots.Clear();

        _onlyBuilderBot = null;
    }

    public void CleareBotList(Bot bot)
    {
        _bots.Remove(bot);
    }

    public void RecieveFirstUsualBots(int counter, Bot bot)
    {
        int botCounter = 0;

        while (botCounter < counter)
        {
            _bots.Add(Instantiate(bot));

            botCounter++;
        }
    }

    public void ChangeBuildStatus()
    {
        HasSentToBuildNewBase = false;
    }

    public void ReceiveBot(Bot bot)
    {
        _bots.Add(bot);
    }

    private IEnumerator SendBotsToBalls()
    {
        while (enabled)
        {
            yield return _wait;

            SendBotToBall();
        }
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

    private void SendBallsToGlobalStorage(Collider[] ballColliders)
    {
        foreach (var ball in ballColliders)
        {
            _gobalStorage.FillFreeBallsCollection(ball);
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

    private void CreateNewUsualBot()
    {
        _counterOfBallForNewBotCreation++;

        if (_counterOfBallForNewBotCreation == _numberBallsToPrepareUsualBot)
        {
            LaunchBot();

            _counterOfBallForNewBotCreation = _startNumberOffUsedBalls;
        }
    }

    private void CreateNewUsualBotFromeExcess()
    {
        if (_counterOfBallForNewBotCreation >= _numberBallsToPrepareUsualBot)
        {
            LaunchBot();

            _counterOfBallForNewBotCreation = _storage.BallCounter;
        }
    }

    private void LaunchBot()
    {
        _botSpawner.CreateNewBot();

        _storage.CorrectRealNumberBallsInStorage(_numberBallsToPrepareUsualBot);
    }


    private void PutNewBotToBotList(Bot bot)
    {
        _bots.Add(bot);

        SetStartPosition(bot);
    }

    private void SetStartPosition(Bot bot)
    {
        bot.transform.position = transform.position;
    }

    private void UseOwnFlag()
    {
        if (_ownFlag != null)
        {
            if (_ownFlag.IsTakenFromBase == false)
            {
                _flagTaker.TakeFlagFromBase(_ownFlag);

                _ownFlag.SetStatusTakenFromBase();

                _ownFlag.SetStatusNotTakenFromField();
            }
            else
            {
                _flagTaker.PutFlagToBase(_ownFlag);

                _ownFlag.SetStatusNotTakenFromBase();

                _ownFlag.SetStatusNotTakenFromField();
            }
        }
    }

    private void StopSearchBuilderBot()
    {
        if (_coroutineForFindingBuilderBot != null)
        {
            StopCoroutine(_coroutineForFindingBuilderBot);

            _coroutineForFindingBuilderBot = null;

            _counterOfBallForNewBotCreation = _storage.BallCounter;

            StartCoroutine(UseStorageOccupancyRate());
        }
    }

    private IEnumerator UseStorageOccupancyRate()
    {
        while (true)
        {
            CreateNewUsualBotFromeExcess();

            yield return null;
        }
    }

    private void LaunchBotToBuildNewBase()
    {
        if (_coroutineForFindingBuilderBot != null)
        {
            StopCoroutine(_coroutineForFindingBuilderBot);

            _coroutineForFindingBuilderBot = null;
        }

        _coroutineForFindingBuilderBot = StartCoroutine(FindFreeBotToBuildNewBase());
    }

    private IEnumerator FindFreeBotToBuildNewBase()
    {
        while (_onlyBuilderBot == null)
        {
            AppointBuilderBot();

            yield return null;
        }

        LaunchToOwnBaseFlag();
    }

    private void AppointBuilderBot()
    {
        if (_storage.BallCounter <= _numberBallsToPrepareBuilderBot)
        {
            _counterOfBallForNewBotCreation = 0;
        }

        if (_bots.Count > 1 && _storage.BallCounter >= _numberBallsToPrepareBuilderBot)
        {
            foreach (var unit in _bots)
            {
                if (_onlyBuilderBot == null)
                {
                    if (unit.IsBusy == false && unit.IsBuilder == false && !HasSentToBuildNewBase)
                    {
                        HasSentToBuildNewBase = true;

                        unit.MakeBusy();

                        unit.MakeBuilder();

                        _builderBots.Add(unit);

                        _onlyBuilderBot = unit;

                        _storage.CorrectRealNumberBallsInStorage(_numberBallsToPrepareBuilderBot);
                    }
                }
            }
        }
    }

    private void LaunchToOwnBaseFlag()
    {
        _onlyBuilderBot.BotRouter.MoveToNewBasePlace(_ownFlag);
    }
}