using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleVision : MonoBehaviour
{
    private Material m_material;
    private bool m_keyPressed = false;

    void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            m_keyPressed = !m_keyPressed;
            m_material.SetFloat("_KeyPressed", m_keyPressed ? 1f : 0f);
        }
    }
}
