using DependencyInjection;
using JetBrains.Annotations;
using UnityEngine;

public class PhotoQuestComponent : ItemInteract
{
    public bool isItemPlaced = false;
    public PhotoFrame photoFrame;
                     
    [SerializeField] public Vector3 PhotoScalePicked;
    [SerializeField] public Vector3 objectPrefabScalePicked;


    public override void Start()
    {
        base.Start();
        PhotoScalePicked = gameObject.transform.localScale;
        objectPrefabScalePicked = objectPrefab.transform.localScale;
        Debug.Log(PhotoScalePicked);
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
            //gameObject.transform.localScale = PhotoScalePicked.localScale;
            photoFrame.RemovePhoto();
            photoFrame = null;
            isItemPlaced = false;
            return base.Interact();
        }
        
    }
}
