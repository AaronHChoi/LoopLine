using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _player;
    public GameObject ActivableBox;

    [SerializeField] private CinemachineCamera Camera;

    [SerializeField] private int speed;

    [SerializeField] private KeyCode Forward;
    [SerializeField] private KeyCode Backwards;
    [SerializeField] private KeyCode Left;
    [SerializeField] private KeyCode Right;
    [SerializeField] private KeyCode Activable;

    void Start()
    {
        _player = GetComponent<CharacterController>();
    }
    public void MoveDir(Vector3 force)
    {
        _player.Move(force);
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
        if (Input.GetKeyDown(Activable))
        {
            ActivableBox.SetActive(!ActivableBox.activeInHierarchy);
        }

        if (dir != Vector3.zero)
        {
            _player.Move(dir.normalized * speed * Time.deltaTime);
        }

        float cameraRotY = Camera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, Camera.transform.eulerAngles.y, 0f);
    }
}
