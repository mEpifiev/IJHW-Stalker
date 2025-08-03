using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class StepDetector : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 0.5f;
    [SerializeField] private LayerMask _layerMask;

    private CapsuleCollider _capsuleCollider;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public bool IsStep(Vector3 direction)
    {
        Vector3 headRayOrigin = new Vector3(transform.position.x, _capsuleCollider.bounds.max.y, transform.position.z);
        Vector3 footRayOrigin = new Vector3(transform.position.x, _capsuleCollider.bounds.min.y, transform.position.z);

        if (Physics.Raycast(footRayOrigin, direction, out RaycastHit footRayHit, _rayDistance, _layerMask) == false)
            return false;

        if (Physics.Raycast(headRayOrigin, direction, out RaycastHit headRayHit, _rayDistance, _layerMask) == false)
            return true;

        if(headRayHit.collider == footRayHit.collider)
            return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        if(_capsuleCollider == null)
            _capsuleCollider = GetComponent<CapsuleCollider>();

        Vector3 direction = transform.forward.normalized;

        Vector3 footRayOrigin = new Vector3(transform.position.x, _capsuleCollider.bounds.min.y, transform.position.z);
        Vector3 headRayOrigin = new Vector3(transform.position.x, _capsuleCollider.bounds.max.y, transform.position.z);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(headRayOrigin, headRayOrigin + direction * _rayDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(footRayOrigin, footRayOrigin + direction * _rayDistance);
    }
}
