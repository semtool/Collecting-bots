using UnityEngine;
using TMPro;

public class Informer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private BallSpawner _ballSpawner;

    private int _counter = 0;

    private void OnEnable()
    {
        _ballSpawner.IsInStorage += ShowNumber;
    }

    private void OnDisable()
    {
        _ballSpawner.IsInStorage += ShowNumber;
    }

    private void ShowNumber()
    {
        _counter++;

        _text.text = _counter.ToString();
    }
}