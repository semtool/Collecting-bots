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

    private List<Base> _realBases = new();
    private float _verticalOffsetForFirstBase = 1.5f;
    private float _verticalOffsetForNewBase = 0.5f;

    private void Start()
    {
        _realBases.Add(BuilldFirstBase());
    }

    private void OnEnable()
    {
        _map.HasNewBaseCoordinatas += AddNewBaseToList;
    }

    private void OnDisable()
    {
        _map.HasNewBaseCoordinatas -= AddNewBaseToList;
    }

    private void AddNewBaseToList(Vector3 vector3, Base parentBase)
    {
        Base newBase = BuildNewBase(vector3, parentBase);

        _realBases.Add(newBase);

        parentBase.CleareBuilderBotList();

        parentBase.ChangeBuildStatus();
    }

    private Base BuilldFirstBase()
    {
        Base firstBase = Instantiate(_basePrefub, new Vector3(0, _verticalOffsetForFirstBase, 0), Quaternion.identity);

        firstBase.SetPool(_ballSpawner);

        firstBase.SetGlobalStorage(_resourceStorage);

        firstBase.RecieveFirstUsualBots(_maxNumberOfBots, _botPfrefub);

        return firstBase;
    }

    private Base BuildNewBase(Vector3 vector, Base parentBase)
    {
        Vector3 basePosition = SetStartPosition(vector);

        Base newBase = Instantiate(_basePrefub, basePosition, Quaternion.identity);

        newBase.SetPool(_ballSpawner);

        newBase.SetGlobalStorage(_resourceStorage);

        ReceiveFirstBotFromParentBase(newBase, parentBase);

        return newBase;
    }

    private Vector3 SetStartPosition(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y + _verticalOffsetForNewBase, vector.z);
    }

    private void ReceiveFirstBotFromParentBase(Base newBase, Base parentBase)
    {
        foreach (var bot in parentBase.BuilderBots)
        {
            if (bot.IsBuilder == true)
            {
                parentBase.CleareBotList(bot);

                bot.MakeNotBuilder();

                newBase.ReceiveBot(bot);
            }
        }
    }
}