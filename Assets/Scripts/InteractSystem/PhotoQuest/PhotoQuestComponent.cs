using JetBrains.Annotations;
using UnityEngine;

public class PhotoQuestComponent : ItemInteract
{
    public bool isItemPlaced = false;
    public PhotoFrame photoFrame;

    public override void Start()
    {
        resetLayerOnPickup = false;
        base.Start();
    }
    public override bool Interact()
    {
        if (!isItemPlaced)
        {
            return base.Interact();
        }
        else
        {
            photoFrame.RemovePhoto();
            photoFrame = null;
            isItemPlaced = false;
            return base.Interact();
        }
    }
}
