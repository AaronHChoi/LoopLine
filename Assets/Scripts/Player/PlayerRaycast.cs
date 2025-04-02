using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{
    public GameObject cellphone;
    public TextMeshProUGUI troughtDisplay;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cellphone.SetActive(!cellphone.activeSelf);
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 30f, Color.green);

        if (Physics.Raycast(ray, out hit, 30f))
        {
            NPC npc = hit.collider.GetComponent<NPC>();

            if(npc != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Debug.Log($"Pensamiento del NPC: {npc.GetThought()}");
                    troughtDisplay.text = npc.GetThought();
                }
            }
        }
    }
}
