using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _botNotBusySpeed;
    [SerializeField] private float _botBusySpeed;

    private float _botSpeed;
    private float _botSpeedOfRotation = 4f;

    public void Move(Vector3 goal)
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, _botSpeed * Time.deltaTime);

        Vector3 direction = goal - transform.position;

        Rotate(direction);
    }

    public void SetNotBusySpeed()
    {
        _botSpeed = _botNotBusySpeed;
    }

    public void SetBusySpeed()
    {
        _botSpeed = _botBusySpeed;
    }

    private void Rotate(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _botSpeedOfRotation);
    }
}