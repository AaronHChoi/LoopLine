using UnityEngine;

public class EagleVision : MonoBehaviour
{
    private bool m_keyPressed = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            m_keyPressed = !m_keyPressed;

            GameObject[] targets = GameObject.FindGameObjectsWithTag("EagleVisionTarget");

            foreach (GameObject target in targets)
            {
                Renderer renderer = target.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.SetFloat("_KeyPressed", m_keyPressed ? 1f : 0f);
                }
            }
        }
    }
}