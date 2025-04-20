using System.Collections.Generic;
using UnityEngine;

namespace Unity.Cinemachine.Samples
{
    /// <summary>
    /// This behaviour makes a GameObject fly around in response to user input.
    /// Movement is relative to the GameObject's local axes.
    /// 
    /// </summary>
    public class asd : MonoBehaviour, Unity.Cinemachine.IInputAxisOwner
    {
        private Transform cameraTransform;
        [Tooltip("Speed when moving")]
        public float Speed = 10;

        [Tooltip("Speed multiplier when sprinting")]
        public float SprintMultiplier = 4;

        [Header("Input Axes")]
        [Tooltip("X Axis movement.  Value is -1..1.  Controls the sideways movement")]
        public InputAxis Sideways = InputAxis.DefaultMomentary;

        [Tooltip("Y Axis movement.  Value is -1..1.  Controls the vertical movement")]
        public InputAxis UpDown = InputAxis.DefaultMomentary;

        [Tooltip("Z Axis movement.  Value is -1..1. Controls the forward movement")]
        public InputAxis Forward = InputAxis.DefaultMomentary;

        [Tooltip("Horizontal rotation.  Value is -1..1.")]
        public InputAxis Pan = DefaultPan;

        [Tooltip("Vertical rotation.  Value is -1..1.")]
        public InputAxis Tilt = DefaultTilt;

        [Tooltip("Sprint movement.  Value is 0 or 1. If 1, then is sprinting")]
        public InputAxis Sprint = InputAxis.DefaultMomentary;

        static InputAxis DefaultPan => new()
        { Value = 0, Range = new Vector2(-180, 180), Wrap = true, Center = 0, Restrictions = InputAxis.RestrictionFlags.NoRecentering };
        static InputAxis DefaultTilt => new()
        { Value = 0, Range = new Vector2(-70, 70), Wrap = false, Center = 0, Restrictions = InputAxis.RestrictionFlags.NoRecentering };
        private void Start()
        {

            cameraTransform = Camera.main.transform;
        }
        /// Report the available input axes to the input axis controller.
        /// We use the Input Axis Controller because it works with both the Input package
        /// and the Legacy input system.  This is sample code and we
        /// want it to work everywhere.
        void IInputAxisOwner.GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
        {
            axes.Add(new() { DrivenAxis = () => ref Sideways, Name = "Move X", Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
            axes.Add(new() { DrivenAxis = () => ref Forward, Name = "Forward" });
            axes.Add(new() { DrivenAxis = () => ref UpDown, Name = "Move Y", Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
            axes.Add(new() { DrivenAxis = () => ref Pan, Name = "Look X (Pan)", Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
            axes.Add(new() { DrivenAxis = () => ref Tilt, Name = "Look Y (Tilt)", Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
            axes.Add(new() { DrivenAxis = () => ref Sprint, Name = "Sprint" });
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
            Speed = 10;
            SprintMultiplier = 4;
            Sideways = InputAxis.DefaultMomentary;
            Forward = InputAxis.DefaultMomentary;
            UpDown = InputAxis.DefaultMomentary;
            Pan = DefaultPan;
            Tilt = DefaultTilt;
            Sprint = InputAxis.DefaultMomentary;
        }

        void OnEnable()
        {
            // Take rotation from the transform
            var euler = transform.rotation.eulerAngles;
            Pan.Value = euler.y;
            Tilt.Value = euler.x;
        }

        void Update()
        {

            // Manejar rotación de la cámara
            Pan.Value += Input.GetAxis("Mouse X") * Speed * Time.deltaTime;
            Tilt.Value -= Input.GetAxis("Mouse Y") * Speed * Time.deltaTime; 

            // Limitar la rotación vertical a un rango definido
            Tilt.Value = Mathf.Clamp(Tilt.Value, -70f, 70f);

            // Aplicar la rotación a la cámara
            var rot = Quaternion.Euler(Tilt.Value, Pan.Value, 0);
            transform.rotation = rot;

        }
    }
}
