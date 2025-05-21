using UnityEngine;
using UnityEngine.VFX;

public class NPCMindPlace : MonoBehaviour
{
    public bool isNPCInteracted;
    public string name;

    private void Awake()
    {
        if(name == GameManager.Instance.workingMan.ToString())
        {
            isNPCInteracted = true;
        }
        if (name == GameManager.Instance.cameraGirl.ToString())
        {
            isNPCInteracted = true;
        }
    }
}
