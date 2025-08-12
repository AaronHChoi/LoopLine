using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }

}