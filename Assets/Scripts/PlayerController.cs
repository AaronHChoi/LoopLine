using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _player;

    [SerializeField] private CinemachineCamera Camera;

    [SerializeField] private int speed;

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
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(Forward))
        {
            dir = transform.forward;
        }
        if (Input.GetKey(Backwards))
        {
            dir = dir - transform.forward;
        }
        if (Input.GetKey(Left))
        {
            dir = dir - transform.right;
        }
        if (Input.GetKey(Right))
        {
            dir = dir + transform.right;
        }

        _player.Move(dir.normalized * speed * Time.deltaTime);

        float cameraRotY = Camera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, Camera.transform.eulerAngles.y, 0f);
    }
}
