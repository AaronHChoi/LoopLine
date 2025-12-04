using System.Collections;
using UnityEngine;

public class RTPCController : MonoBehaviour
{
    public AK.Wwise.RTPC rtpc;
    public AK.Wwise.Event rtpcEvent;

    public float rtpcValue = 100f; // Valor final de ejemplo
    public float rtpcTime = 4f;
    public float speed = 1.5f;

    private float currentValue = 0f;
    private float elapsed = 40f;

    private bool isIncreasing = true;

    private void Start()
    {
        currentValue = 0f;
        elapsed = 0f;

        if (rtpc != null)
        {
            rtpc.SetValue(this.gameObject, currentValue);
        }
        if (rtpcEvent != null)
        {
            rtpcEvent.Post(this.gameObject);
        }
    }

    private void Update()
    {
        if (rtpc == null)
            return;

        if (elapsed < rtpcTime && isIncreasing)
        {
            elapsed += Time.deltaTime * speed;
            rtpcValue = elapsed;
            rtpc.SetValue(this.gameObject, rtpcValue);
            
            if (elapsed >= rtpcTime)
            {
                isIncreasing = false;
            }
        }
        else if (elapsed >= 0 && !isIncreasing)
        {
            elapsed -= Time.deltaTime * speed;
            rtpcValue = elapsed;
            rtpc.SetValue(this.gameObject, rtpcValue);

            if (elapsed <= 0f)
            {
                isIncreasing = true;
            }
        }
        //UpdateValue();
    }

    public IEnumerator WaitforSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }
    private void UpdateValue()
    {
        rtpc.SetValue(this.gameObject, rtpcValue);
        Debug.Log("RTPC Value: " + currentValue);
        Debug.Log(rtpcEvent);
    }
}
