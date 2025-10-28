using UnityEngine;
using UnityEngine.Events;

public class GenericMove : MonoBehaviour
{
    public enum MoveType { Position, Rotation, Scale }
    public enum MoveMode { Absolute, Relative }

    [Header("Target Settings")]
    [SerializeField] private Transform target;

    [Header("Movement Settings")]
    [SerializeField] private MoveType moveType = MoveType.Position;
    [SerializeField] private MoveMode moveMode = MoveMode.Relative;
    [SerializeField] private Vector3 moveAmount = new Vector3(0, 1, 0);
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Trigger Settings")]
    [SerializeField] private bool moveOnStart = false;
    public UnityEvent OnMoveCompleted;

    private bool isMoving = false;
    private Vector3 startValue;
    private Vector3 targetValue;
    private float timer;

    private void Start()
    {
        if (target == null)
            target = transform;

        if (moveOnStart)
            Move();
    }
    public void Move()
    {
        if (isMoving || target == null) return;

        isMoving = true;
        timer = 0f;

        switch (moveType)
        {
            case MoveType.Position:
                startValue = target.position;
                targetValue = moveMode == MoveMode.Relative ? target.position + moveAmount : moveAmount;
                break;

            case MoveType.Rotation:
                startValue = target.eulerAngles;
                targetValue = moveMode == MoveMode.Relative ? startValue + moveAmount : moveAmount;
                break;

            case MoveType.Scale:
                startValue = target.localScale;
                targetValue = moveMode == MoveMode.Relative ? startValue + moveAmount : moveAmount;
                break;
        }

        StartCoroutine(DoMove());
    }
    private System.Collections.IEnumerator DoMove()
    {
        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = moveCurve.Evaluate(timer / moveDuration);

            switch (moveType)
            {
                case MoveType.Position:
                    target.position = Vector3.LerpUnclamped(startValue, targetValue, t);
                    break;

                case MoveType.Rotation:
                    target.rotation = Quaternion.LerpUnclamped(
                        Quaternion.Euler(startValue),
                        Quaternion.Euler(targetValue),
                        t
                    );
                    break;

                case MoveType.Scale:
                    target.localScale = Vector3.LerpUnclamped(startValue, targetValue, t);
                    break;
            }

            yield return null;
        }

        switch (moveType)
        {
            case MoveType.Position: target.position = targetValue; break;
            case MoveType.Rotation: target.rotation = Quaternion.Euler(targetValue); break;
            case MoveType.Scale: target.localScale = targetValue; break;
        }

        isMoving = false;
        OnMoveCompleted?.Invoke();
    }
}