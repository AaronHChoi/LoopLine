using DependencyInjection;
using UnityEngine;

public class BlackRoomComponent : MonoBehaviour, IBlackRoomComponent
{
    public bool IsActive { get; set; } = false;

    [SerializeField] public GameObject ObjectToActivate { get; set; }
    [SerializeField] public AudioSource AudioSource;

    IBlackRoomManager BKRoomManager;

    private void Awake()
    {
        BKRoomManager = InterfaceDependencyInjector.Instance.Resolve<IBlackRoomManager>();
        AudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (ObjectToActivate == null && transform.childCount > 0)
        {
            ObjectToActivate = transform.GetChild(0).gameObject;
        }
        else
        {
            ObjectToActivate = gameObject;
        }
    }
}

public interface IBlackRoomComponent
{
    bool IsActive { get; set; }
    public GameObject ObjectToActivate { get; set; }
}
