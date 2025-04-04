using System;
using UnityEngine;

public class FlagTaker : MonoBehaviour
{
    public event Action<Flag> FlagIsTaken;

    public void TakeFlagFromBase(Flag flag)
    {
        FlagIsTaken?.Invoke(flag);

        flag.ColorChanger.IncreaseAlfa();

        flag.TurnOnTransporter();

        flag.SetStatusNotTakenFromBase();
    }

    public void PutFlagToBase(Flag flag)
    {
        flag.TurnOffTransporter();

        flag.SetStartCoordinates();

        flag.InformBase();
    }
}