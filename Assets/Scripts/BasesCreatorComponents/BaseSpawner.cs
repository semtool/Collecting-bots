using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private BallSpawner _ballSpawner;
    [SerializeField] private GlobalResourceStorage _resourceStorage;
    [SerializeField] private Base _basePrefub;
    [SerializeField] private Map _map;
    [SerializeField] private Bot _botPfrefub;
    [SerializeField] private int _maxNumberOfBots;

    private float _verticalOffsetForFirstBase = 1.5f;
    private float _verticalOffsetForNewBase = 0.5f;

    private void Start()
    {
        BuilldFirstBase();
    }

    private void OnEnable()
    {
        _map.HasNewBaseCoordinatas += BuildNewBase;
    }

    private void OnDisable()
    {
        _map.HasNewBaseCoordinatas -= BuildNewBase;
    }

    private void BuildNewBase(Vector3 vector3, Base parentBase)
    {
        CreateNewBase(vector3, parentBase);

        parentBase.CleareBuilderBotList();

        parentBase.ChangeBuildStatus();
    }

    private void BuilldFirstBase()
    {
        Base firstBase = Instantiate(_basePrefub, new Vector3(0, _verticalOffsetForFirstBase, 0), Quaternion.identity);

        firstBase.SetPool(_ballSpawner);

        firstBase.SetGlobalStorage(_resourceStorage);

        firstBase.RecieveFirstUsualBots(_maxNumberOfBots, _botPfrefub);
    }

    private void CreateNewBase(Vector3 vector, Base parentBase)
    {
        Vector3 basePosition = SetStartPosition(vector);

        Base newBase = Instantiate(_basePrefub, basePosition, Quaternion.identity);

        newBase.SetPool(_ballSpawner);

        newBase.SetGlobalStorage(_resourceStorage);

        ReceiveFirstBotFromParentBase(newBase, parentBase);
    }

    private Vector3 SetStartPosition(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y + _verticalOffsetForNewBase, vector.z);
    }

    private void ReceiveFirstBotFromParentBase(Base newBase, Base parentBase)
    {
        if(parentBase.OnlyBuilderBot != null)
        {
            parentBase.OnlyBuilderBot.MakeNotBuilder();

            Bot bot = parentBase.OnlyBuilderBot;

            parentBase.CleareBotList(parentBase.OnlyBuilderBot);

            newBase.ReceiveBot(bot);
        }
    }
}