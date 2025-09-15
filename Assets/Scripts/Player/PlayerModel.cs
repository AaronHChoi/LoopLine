using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModel", menuName = "Scriptable Object/PlayerModel")]
public class PlayerModel : ScriptableObject
{
    [SerializeField] float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField] float sprintSpeed;
    public float SprintSpeed { get => sprintSpeed; private set => sprintSpeed = value; }

    [SerializeField] float speedRotation;
    public float SpeedRotation { get => speedRotation; private set => speedRotation = value; }

    [SerializeField] float lookSensitivity;
    public float LookSensitivity { get => lookSensitivity; private set => lookSensitivity = value; }


    [SerializeField] float yAxisLocation;
    public float YAxisLocation { get => yAxisLocation; private set => yAxisLocation = value; }
}