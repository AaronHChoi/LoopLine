using UnityEngine;
using AK.Wwise;

public class TriggerChange : MonoBehaviour
{
    [Header("States")]
    public State defaultState;
    public State newState;
    private void OnTriggerEnter(Collider other)
    {
        newState.SetValue();
    }

    private void OnTriggerExit(Collider other)
    {
        defaultState.SetValue();
    }
}
