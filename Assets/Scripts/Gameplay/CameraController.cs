using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineCamera player;
    [SerializeField] CinemachineCamera sitPlayer;
    bool active = true;
    private void Start()
    {
        sitPlayer.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && active)
        {
            sitPlayer.gameObject.SetActive(false);
            active = false;
        }
    }
}