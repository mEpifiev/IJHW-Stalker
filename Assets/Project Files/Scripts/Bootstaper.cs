using UnityEngine;

public class Bootstaper : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerController;
    [SerializeField] private Follower _follower;

    private void Awake()
    {
        _follower.SetTarget(_playerController);
    }
}
