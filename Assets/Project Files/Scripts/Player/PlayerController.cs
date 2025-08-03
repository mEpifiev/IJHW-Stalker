using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IMoveable
{
    private const float GroundStickForce = -1f;
    private const float MaxLookAngle = 90f;
    private const float MinLookAngle = -90f;

    [Header("Dependencies")]
    [SerializeField] private InputReader _inputReader;

    [Space(2), Header("Settings")]
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _jumpForce = 9f;
    [SerializeField] private float _horizontalTurnSensitivity = 4f;
    [SerializeField] private float _verticalTurnSensitivity = 2f;
    [SerializeField] private float _gravityFactor = 2f;
    [SerializeField] private bool _isCursorHidden;

    private CharacterController _characterController;
    private Camera _camera;

    private Vector3 _direction;
    private Vector2 _mouseLookDelta;
    private Vector3 _velocity;
    private float _cameraXAngle;

    public Transform Transform { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Transform = transform;
        _camera = Camera.main;

        if (_isCursorHidden)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnEnable()
    {
        _inputReader.Moved += OnMove;
        _inputReader.Looked += OnLook;
        _inputReader.Jumped += OnJump;
    }

    private void OnDisable()
    {
        _inputReader.Moved -= OnMove;
        _inputReader.Looked -= OnLook;
        _inputReader.Jumped -= OnJump;
    }

    private void Update()
    {
        ApplyGravity();
        Move();
        RotateCamera();
    }   

    private void Move()
    {
        Vector3 moveDirection = transform.TransformDirection(_direction) * _moveSpeed;

        _characterController.Move((moveDirection + _velocity) * Time.deltaTime);
    }

    private void RotateCamera()
    {
        float mouseX = _mouseLookDelta.x * _horizontalTurnSensitivity;
        float mouseY = _mouseLookDelta.y * _verticalTurnSensitivity;

        Transform.Rotate(mouseX * Vector3.up);

        _cameraXAngle -= mouseY;
        _cameraXAngle = Mathf.Clamp(_cameraXAngle, MinLookAngle, MaxLookAngle);

        _camera.transform.localEulerAngles = Vector3.right * _cameraXAngle;

        _mouseLookDelta = Vector2.zero;
    }

    private void OnMove(Vector3 direction)
    {
        _direction = direction;
    }

    private void OnLook(Vector2 mouseLookDelta)
    {
        _mouseLookDelta = mouseLookDelta;
    }

    private void OnJump()
    {
        if (_characterController.isGrounded)
            _velocity.y = _jumpForce;
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0f)
            _velocity.y = GroundStickForce;
        else
            _velocity.y += Physics.gravity.y * _gravityFactor * Time.deltaTime;
    }
}
