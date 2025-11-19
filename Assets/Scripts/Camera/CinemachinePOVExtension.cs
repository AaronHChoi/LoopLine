using System.Collections.Generic;
using UnityEngine;
using DependencyInjection;
using UnityEngine.InputSystem;

namespace Unity.Cinemachine.Samples
{
    public class CinemachinePOVExtension : MonoBehaviour, Unity.Cinemachine.IInputAxisOwner, ICameraOrientation
    {
        IPlayerController controller;
        IPlayerInputHandler inputHandler;

        bool canLook = true;
        public bool CanLook { get => canLook; set => canLook = value;}

        [Header("Input Axes")]
        [Tooltip("Horizontal rotation.  Value is -1..1.")]
        public InputAxis Pan = DefaultPan;

        [Tooltip("Vertical rotation.  Value is -1..1.")]
        public InputAxis Tilt = DefaultTilt;

        static InputAxis DefaultPan => new()
        { Value = 0, Range = new Vector2(-180, 180), Wrap = true, Center = 0, Restrictions = InputAxis.RestrictionFlags.NoRecentering };
        static InputAxis DefaultTilt => new()
        { Value = 0, Range = new Vector2(-70, 70), Wrap = false, Center = 0, Restrictions = InputAxis.RestrictionFlags.NoRecentering };
        private void Awake()
        {
            inputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
            controller = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        }
        void IInputAxisOwner.GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
        {
            axes.Add(new() { DrivenAxis = () => ref Pan, Name = "Look X (Pan)", Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
            axes.Add(new() { DrivenAxis = () => ref Tilt, Name = "Look Y (Tilt)", Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
        }
        void OnValidate()
        {
            Pan.Validate();
            Tilt.Range.x = Mathf.Clamp(Tilt.Range.x, -90, 90);
            Tilt.Range.y = Mathf.Clamp(Tilt.Range.y, -90, 90);
            Tilt.Validate();
        }
        void Reset()
        {
            Pan = DefaultPan;
            Tilt = DefaultTilt;
        }
        void OnEnable()
        {
            var euler = transform.rotation.eulerAngles;
            Pan.Value = euler.y;
            Tilt.Value = euler.x;
        }
        private void LateUpdate()
        {
            HandleLook();
        }
        public void HandleLook()
        {
            if (!canLook) return;

            var gamepad = Gamepad.current;

            if (gamepad != null)
            {
                Vector2 stick = gamepad.leftStick.ReadValue();
                Vector2 mov = stick * 2f;
            }

            Vector2 lookDelta = inputHandler.GetInputDelta();
            // Manejar rotación de la cámara
            Pan.Value += lookDelta.x * controller.PlayerModel.LookSensitivity * Time.deltaTime;
            Tilt.Value -= lookDelta.y * controller.PlayerModel.LookSensitivity * Time.deltaTime;

            // Limitar la rotación vertical a un rango definido
            Tilt.Value = Mathf.Clamp(Tilt.Value, -70f, 70f);

            // Aplicar la rotación a la cámara
            transform.rotation = Quaternion.Euler(Tilt.Value, Pan.Value, 0);
        }
        public (float pan, float tilt) GetPanAndTilt()
        {
            return (Pan.Value, Tilt.Value);
        }
        public void SetPanAndTilt(float pan, float tilt)
        {
            Pan.Value = Mathf.Clamp(pan, Pan.Range.x, Pan.Range.y);
            Tilt.Value = Mathf.Clamp(tilt, Tilt.Range.x, Tilt.Range.y);
        }
    }
}
public interface ICameraOrientation
{
    (float pan, float tilt) GetPanAndTilt();

    public bool CanLook { get; set; }
    void SetPanAndTilt(float pan, float tilt);
}