using UnityEngine;

public class CarriageController : MonoBehaviour
{
    public float Speed;
    public bool Move;

    CharacterController _carriage;
    
    void Start()
    {
        _carriage = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Move)
        {
            _carriage.Move(transform.forward * Speed * Time.deltaTime);
        }
    }
}
