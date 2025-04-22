using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private StructScreens[] screens;
    private Stack<IScreen> screenStack = new Stack<IScreen>();
    private Dictionary<EnumScreenName, IScreen> dictionaryScreens = new Dictionary<EnumScreenName, IScreen>();

    private void Awake()
    {
        for (int i = 0; i < screens.Length; i++)
        {
            dictionaryScreens[screens[i].ScreenName] = screens[i].ScreenObject.GetComponent<IScreen>();
            screens[i].ScreenObject.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        Push(EnumScreenName.Gameplay);
    }
    public void Pop()
    {
        if (screenStack.Count <= 1) return;

        screenStack.Pop().Free();
        screenStack.Peek().Activate();
    }
    public void Push(EnumScreenName _screen)
    {
        if (screenStack.Count > 0)
            screenStack.Peek().Deactivate();

        screenStack.Push(dictionaryScreens[_screen]);
        dictionaryScreens[_screen].Activate();
    }
}