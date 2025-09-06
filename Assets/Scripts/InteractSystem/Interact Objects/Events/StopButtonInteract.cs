using UnityEngine;
using DependencyInjection;
public class StopButtonInteract : MonoBehaviour, IInteract, IDependencyInjectable
{

    [Header("Sound System")]
    [SerializeField] private SoundData BreakSecurityCrystal;
    [SerializeField] private SoundData PushButton;


    [SerializeField] private string interactText = "";
    [SerializeField] private GameObject Crystal;
    private bool hasStoped = false;

    [SerializeField] public GameObject TriggerRock;
    [SerializeField] public ItemInteract Rock;

    ItemManager itemManager;
    InventoryUI inventoryUI;
    EventManager eventManager;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }

    private void Start()
    {
        foreach (var item in itemManager.items) 
        {
            if (item.id == "Rock") //asegurarse que el item de la roca tenga este id
            {
                Rock = item;
            }
        }
    }
    public void Interact()
    {
        if (!hasStoped)
        {
            if (Crystal.gameObject.activeSelf == true)
            {

                if (inventoryUI.ItemInUse == Rock)
                {

                    SoundManager.Instance.CreateSound()
                        .WithSoundData(BreakSecurityCrystal)
                        .Play();
                    Crystal.gameObject.SetActive(false);
                    inventoryUI.RemoveInventorySlot(Rock);

                }
            }
            else
            {
                SoundManager.Instance.CreateSound()
                        .WithSoundData(PushButton)
                        .Play();

                eventManager.TrainEventStopTrain();
                gameObject.layer = LayerMask.NameToLayer("Default");
                hasStoped = true;
            }
        }
    }

    void Update()
    {
        if (eventManager.stopTrain == true)
        {
            eventManager.StopedTimeForTrain -= Time.deltaTime;
            if(eventManager.StopedTimeForTrain < 0) eventManager.StopedTimeForTrain = 0;
        }       
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public void InjectDependencies(DependencyContainer provider)
    {
        inventoryUI = provider.UIContainer.InventoryUI;
        eventManager = provider.ManagerContainer.EventManager;
        itemManager = provider.ManagerContainer.ItemManager;
    }
}
