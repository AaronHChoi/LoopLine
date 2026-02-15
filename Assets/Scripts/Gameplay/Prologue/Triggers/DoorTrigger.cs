using UnityEngine;
using Core.Data;
using Core.Utilities;

public class DoorTrigger : BaseTrigger
{
    protected override void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SetCondition(GameCondition.PrologueDoorsLocked, true);

        base.OnTriggerEnter(other);
    }
}