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

    [Header("Head Bob Settings")]
    [SerializeField] float walkBobAmplitudeGain = 0.5f;
    public float WalkBobAmplitudeGain { get => walkBobAmplitudeGain; private set => walkBobAmplitudeGain = value; }

    [SerializeField] float walkBobFrequencyGain = 0.5f;
    public float WalkBobFrequencyGain { get => walkBobFrequencyGain; private set => walkBobFrequencyGain = value; }

    [SerializeField] float sprintBobAmplitudeGain = 1.0f;
    public float SprintBobAmplitudeGain { get => sprintBobAmplitudeGain; private set => sprintBobAmplitudeGain = value; }

    [SerializeField] float sprintBobFrequencyGain = 1.0f;
    public float SprintBobFrequencyGain { get => sprintBobFrequencyGain; private set => sprintBobFrequencyGain = value; }

    [SerializeField] float bobSmoothTime = 10f;
    public float BobSmoothTime { get => bobSmoothTime; private set => bobSmoothTime = value; }
}