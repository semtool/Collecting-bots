using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<Renderer>();
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }

    public void IncreaseAlfa()
    {
        Color selfColor = _renderer.material.color;

        selfColor.a = 1;

        _renderer.material.color = selfColor;
    }

    public void DecreaseAlfa()
    {
        Color selfColor = _renderer.material.color;

        selfColor.a = 0.2f;

        _renderer.material.color = selfColor;
    }
}