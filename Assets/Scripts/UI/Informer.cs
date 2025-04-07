using UnityEngine;
using TMPro;

public class Informer : MonoBehaviour
{
    [SerializeField] private BaseStorage _baseStorage;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _baseStorage.IsCountedForInformer += ShowNumber;

        _baseStorage.IsCorrected += ShowNumber;
    }

    private void OnDisable()
    {
        _baseStorage.IsCountedForInformer -= ShowNumber;

        _baseStorage.IsCorrected -= ShowNumber;
    }

    private void ShowNumber(int counter)
    {
        _text.text = counter.ToString();
    }
}