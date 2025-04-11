using UnityEngine;

public class HitHandler : MonoBehaviour
{
    [SerializeField] private RayCaster _rayCaster;
    [SerializeField] private LayerMask _layerMask;

    private Base _home = null;
    private Flag _flag = null;


    private void OnEnable()
    {
        _rayCaster.WasHit += OperateWithObjects;
    }

    private void OnDisable()
    {
        _rayCaster.WasHit -= OperateWithObjects;
    }

    private void OperateWithObjects(RaycastHit raycastHit)
    {
        if (raycastHit.collider.gameObject.TryGetComponent(out Base home))
        {
            _home = home;

            _flag = home.Flag;

            _home.UseOwnFlag();
        }

        if (raycastHit.collider.gameObject.TryGetComponent(out Flag flag))
        {
            _flag = flag;

            _flag.MoveFlag();
        }

        if (raycastHit.collider.gameObject.TryGetComponent(out Map field))
        {
            if (_flag != null)
            {
                _flag.SendSignalToLaunchBot(field, raycastHit.point);
            }
        }
    }
}
