using UnityEngine;
using System;
public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefub;

    public event Action<Bot> IsCreated;

    public void CreateNewBot()
    {
        IsCreated?.Invoke(CreateBot());
    }

    private Bot CreateBot()
    {
        Bot bot = Instantiate(_botPrefub);

        return bot;
    }
}