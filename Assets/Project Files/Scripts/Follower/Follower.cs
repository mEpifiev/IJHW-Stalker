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
    private Vector3 _direction;

    private ITarget _target;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetTarget(ITarget moveable)
    {
        _target = moveable;
    }

    private void Update()
    {
        RotateTo();
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

        _direction = toTarget.normalized;

        if (_stepDetector.IsStep(_direction))
            _rigidbody.AddForce(Vector3.up * _stepUpForce, ForceMode.VelocityChange);

        _rigidbody.velocity = _direction * _moveSpeed;
    }

    private void RotateTo()
    {
        const float MinRotationThreshold = 0.01f;

        if (_direction.sqrMagnitude < MinRotationThreshold)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(_direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        if(_groundDetector.IsGrounded())
            return;

        _rigidbody.velocity += Physics.gravity;
    }
}
