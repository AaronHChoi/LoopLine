using Core.Data;
using Core.DependencyInjection;
using Core.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class NPCSceneController : MonoBehaviour
{
    private const int TOTAL_REQUIRED = 3;
    private List<NPCType> npcsTakedTo = new List<NPCType>();

    IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    public void OnNpcTaked(NPCType type)
    {
        if (!npcsTakedTo.Contains(type))
        {
            npcsTakedTo.Add(type);

            if (npcsTakedTo.Count >= TOTAL_REQUIRED)
            {
                GameManager.Instance.SetCondition(GameCondition.AllNpcsLockedSpoken, true);
                GameManager.Instance.SetCondition(GameCondition.PrologueDoorsLocked, false);
               
                TriggerEndSceneMonologue();
            }
        }
    }
    private void TriggerEndSceneMonologue()
    {
        DelayUtility.Instance.Delay(3f, () => monologueSpeaker.StartMonologue(Events.NPCL_AllNPCSpoken));
    }
#if UNITY_EDITOR
    #region DEBUG_TOOLS
    [ContextMenu("Debug: Force Complete and start monologue")]
    private void DebugForceComplete()
    {
        GameManager.Instance.SetCondition(GameCondition.AllNpcsLockedSpoken, true);
        GameManager.Instance.SetCondition(GameCondition.PrologueDoorsLocked, false);
      
        TriggerEndSceneMonologue();
    }
    #endregion
#endif
}