using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Transform _feetPosition;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _radius = 0.2f;

    public bool IsGrounded()
    {
        Collider[] colldiers = Physics.OverlapSphere(_feetPosition.position, _radius, _layerMask);

        return colldiers.Length > 0;
    }
}
