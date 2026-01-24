using UnityEngine;
using Gameplay.DependencyInjection;

public class BlackRoomComponent : MonoBehaviour, IBlackRoomComponent
{
    public bool IsActive { get; set; } = false;

    [SerializeField] public GameObject ObjectToActivate { get; set; }
    [SerializeField] public AudioSource AudioSource;

    IBlackRoomManager BKRoomManager;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        BKRoomManager = InterfaceDependencyInjector.Instance.Resolve<IBlackRoomManager>();

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