using UnityEngine;

public class Teste : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            NPCDialogueManager.Instance.HandleEventChange(NPCType.CameraGirl, Events.Test);
        }
    }
}
