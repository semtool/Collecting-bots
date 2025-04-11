using System;
using UnityEngine;

public class FlagTaker : MonoBehaviour
{
    public event Action<Flag> FlagIsTaken;

    public void TakeFlagFromBase(Flag flag)
    {
        flag.ColorChanger.IncreaseAlfa();

        flag.TurnOnTransporter();
    }

    public void PutFlagToBase(Flag flag)
    {
        flag.TurnOffTransporter();

        flag.SetStartCoordinates();

        flag.InformBase();
    }
}