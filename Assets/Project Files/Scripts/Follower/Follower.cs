using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Follower : MonoBehaviour
{
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private StepDetector _stepDetector;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _stoppingDistance = 2f;
    [SerializeField] private float _stepUpForce = 2.5f;

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;

    private IMoveable _target;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void SetTarget(IMoveable moveable)
    {
        _target = moveable;
    }

    private void FixedUpdate()
    {
        if (_target == null)
            return;
        
        Follow();
        ApplyGravity();
    }

    private void Follow()
    {
        Vector3 toTarget = _target.Transform.position - transform.position;
        toTarget.y = 0;

        if (toTarget.magnitude <= _stoppingDistance)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        Vector3 direction = toTarget.normalized;

        RotateTo(direction);

        if (_stepDetector.IsStep(direction))
            _rigidbody.AddForce(Vector3.up * _stepUpForce, ForceMode.VelocityChange);

        _rigidbody.velocity = direction * _moveSpeed;
    }

    private void RotateTo(Vector3 direction)
    {
        const float minRotationThreshold = 0.01f;

        if (direction.sqrMagnitude < minRotationThreshold)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        if(_groundDetector.IsGrounded())
            return;

        _rigidbody.velocity += Physics.gravity;
    }
}
