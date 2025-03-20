using UnityEngine;

public class Mover : MonoBehaviour
{
    private float _botSpeed;
    private float _botSpeedOfRotation = 4f;
    private float _botNotBusySpeed = 6f;
    private float _botBusySpeed = 2f;

    public void Move(Vector3 goal)
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, _botSpeed * Time.deltaTime);

        Vector3 direction = goal - transform.position;

        Rotate(direction);
    }

    private void Rotate(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _botSpeedOfRotation);
    }

    public void SetNotBusySpeed()
    {
        _botSpeed = _botNotBusySpeed;
    }

    public void SetBusySpeed()
    {
        _botSpeed = _botBusySpeed;
    }
}