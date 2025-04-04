using System;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private BaseSpawner _baseSpawner;

    public event Action<Vector3, Base> HasNewBaseCoordinatas;
    public event Action<Bot> IsGotFromParentBase;

    public Bot Bot { get; private set; }

    public void SendGoordinatesToBaseSpawner(Vector3 vector, Base parentBase)
    {
        HasNewBaseCoordinatas?.Invoke(vector, parentBase);
    }
}