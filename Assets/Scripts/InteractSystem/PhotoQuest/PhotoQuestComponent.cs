using DependencyInjection;
using JetBrains.Annotations;
using UnityEngine;

public class PhotoQuestComponent : ItemInteract
{
    public bool isItemPlaced = false;
    public PhotoFrame photoFrame;
    [SerializeField] public Transform PhotoScalePicked;

    public override void Start()
    {
        base.Start();
        PhotoScalePicked = gameObject.transform;
        PhotoScalePicked.localScale = gameObject.transform.localScale;
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
            gameObject.transform.localScale = PhotoScalePicked.localScale;
            photoFrame.RemovePhoto();
            photoFrame = null;
            isItemPlaced = false;
            return base.Interact();
        }
        
    }
}
