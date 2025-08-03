using System;
using System.Linq;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private readonly string Horizontal = "Horizontal";
    private readonly string Vertical = "Vertical";
    private readonly string MouseX = "Mouse X";
    private readonly string MouseY = "Mouse Y";
    private readonly string Jump = "Jump";

    private Vector3 _direction;
    private Vector2 _mouseLookDelta;

    public event Action<Vector3> Moved;
    public event Action<Vector2> Looked;
    public event Action Jumped;

    private void Update()
    {
        _direction = new Vector3(Input.GetAxis(Horizontal), 0f, Input.GetAxis(Vertical));
        _mouseLookDelta = new Vector2(Input.GetAxis(MouseX), Input.GetAxis(MouseY));

        if(_direction.sqrMagnitude > 0f || _mouseLookDelta.sqrMagnitude > 0f)
        {
            Moved?.Invoke(_direction);
            Looked?.Invoke(_mouseLookDelta);
        }

        if (Input.GetButtonDown(Jump))
        {
            Jumped?.Invoke();
        }
    }
}
