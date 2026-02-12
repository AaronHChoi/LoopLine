using Core.Audio;
using Core.DependencyInjection;
using Core.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ISoundManager soundManager;
    IMenuManager menuManager;

    [SerializeField] private GameObject HoverGameObject;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        menuManager = InterfaceDependencyInjector.Instance.Resolve<IMenuManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuManager.HoverBehaviour();
        if (HoverGameObject != null)
        {
            HoverGameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HoverGameObject != null)
        {
            HoverGameObject.SetActive(false);
        }
    }

}
