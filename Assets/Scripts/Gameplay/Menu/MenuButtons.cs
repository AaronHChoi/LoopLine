using Core.Audio;
using Core.DependencyInjection;
using Core.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler
{
    ISoundManager soundManager;
    IMenuManager menuManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        menuManager = InterfaceDependencyInjector.Instance.Resolve<IMenuManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuManager.HoverBehaviour();
    }

}
