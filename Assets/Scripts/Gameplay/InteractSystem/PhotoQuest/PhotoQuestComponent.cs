
public class PhotoQuestComponent : ItemInteract
{
    public bool isItemPlaced = false;
    public PhotoFrame photoFrame;                    

    public override void Start()
    {
        base.Start();
       
        this.id = ItemData.itemName;    
        resetLayerOnPickup = false;
    }
    public override bool Interact()
    {
        if (!isItemPlaced)
        {
            EventBus.Publish(new PlayerGrabItemEvent());
            return base.Interact();
        }
        else 
        {
            photoFrame = null;
            isItemPlaced = false;
            EventBus.Publish(new PlayerGrabItemEvent());
            return base.Interact();
        }
    }
}