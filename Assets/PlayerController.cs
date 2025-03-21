using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _player;

    [SerializeField] private int _speed = 1;
    [SerializeField] private KeyCode Forward;
    [SerializeField] private KeyCode Backwards;
    [SerializeField] private KeyCode Left;
    [SerializeField] private KeyCode Right;

    void Start()
    {
        _player = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKey(Forward))
        {
            _player.Move(Vector3.forward * _speed * Time.deltaTime);
        }
        if (Input.GetKey(Backwards))
        {
            _player.Move(-Vector3.forward * _speed * Time.deltaTime);
        }
        if (Input.GetKey(Left))
        {
            _player.Move(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKey(Right))
        {
            _player.Move(Vector3.right * _speed * Time.deltaTime);
        }
    }
}
