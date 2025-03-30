using UnityEngine;

public class CarriageController : MonoBehaviour
{

    public float Speed;
    public PlayerController Player;

    private bool _move;
    CharacterController _carriage;
    
    void Start()
    {
        _carriage = GetComponent<CharacterController>();
    }
    public void MoveCarriage(bool move)
    {
        _move = move;
        if (move)
        {
            Player.transform.SetParent(transform);
        }
        else
        {
            Player.transform.SetParent(null);
            //Player.StopMoving();
        }
    }
    void Update()
    {
        if (_move)
        {
            Vector3 force = transform.forward * Speed * Time.deltaTime;
            _carriage.Move(force);
            //Player.MoveDir(force);
        }
    }
}
