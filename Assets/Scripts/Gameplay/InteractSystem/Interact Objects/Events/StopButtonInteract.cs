using UnityEngine;
using Core.Audio;
using Core.DependencyInjection;

public class StopButtonInteract : MonoBehaviour, IInteract
{
    [Header("Sound System")]
    [SerializeField] private SoundData BreakSecurityCrystal;
    [SerializeField] private SoundData PushButton;

    [SerializeField] private string interactText = "";
    [SerializeField] private GameObject Crystal;
    private bool hasStoped = false;

    [SerializeField] public GameObject TriggerRock;
    [SerializeField] public ItemInteract Rock;

    IItemManager itemManager;
    IInventoryUI inventoryUI;
    IEventManager eventManager;
    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        itemManager = InterfaceDependencyInjector.Instance.Resolve<IItemManager>();
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        eventManager = InterfaceDependencyInjector.Instance.Resolve<IEventManager>();
    }
    private void Start()
    {
        //foreach (var item in itemManager.items) 
        //{
        //    if (item.id == "Rock") //asegurarse que el item de la roca tenga este id
        //    {
        //        Rock = item;
        //    }
        //}
    }
    public void Interact()
    {
        if (!hasStoped)
        {
            //if (Crystal.gameObject.activeSelf == true)
            //{

            //    if (inventoryUI.ItemInUse == Rock)
            //    {

            //        soundManager.CreateSound()
            //            .WithSoundData(BreakSecurityCrystal)
            //            .Play();
            //        Crystal.gameObject.SetActive(false);
            //        inventoryUI.RemoveInventorySlot(Rock);

            //    }
            //}
            //else
            //{
                soundManager.CreateSound()
                        .WithSoundData(PushButton)
                        .Play();

                eventManager.TrainEventStopTrain();
                gameObject.layer = LayerMask.NameToLayer("Default");
                hasStoped = true;
            //}
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
}