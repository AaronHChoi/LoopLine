public class PlayerModel
{
    public float Speed { get; private set; } = 3f;
    public float SpeedRotation { get; private set; } = 10f;
    public float LookSensitivity { get; private set; } = 4.5f;
    public bool FocusMode { get; set; } = true;
}
