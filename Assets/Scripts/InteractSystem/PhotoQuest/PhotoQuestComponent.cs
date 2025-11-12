using DependencyInjection;
using JetBrains.Annotations;
using UnityEngine;

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
            return base.Interact();
        }
        else 
        {
            photoFrame = null;
            isItemPlaced = false;
            return base.Interact();
        }
        
    }
}
