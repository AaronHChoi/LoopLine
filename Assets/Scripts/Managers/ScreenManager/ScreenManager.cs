using System.Collections.Generic;
using UnityEngine;

public enum EnumScreenName
{
    MainMenu, GamePlay, Pause
}
public class ScreenManager : MonoBehaviour, IScreenManager
{
    [SerializeField] private StructScreens[] _screens;
    private Stack<IScreen> _screenStack = new Stack<IScreen>(); 
    private Dictionary<EnumScreenName, IScreen> _dictionaryScreens = new Dictionary<EnumScreenName, IScreen>();
    private void Awake()
    {
        for (int i = 0; i < _screens.Length; i++)
        {
            _dictionaryScreens[_screens[i].screenName] = _screens[i].screenObject.GetComponent<IScreen>(); // Cargamos los datos al diccionario.
            _screens[i].screenObject.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        Push(EnumScreenName.GamePlay);
    }

    public void Pop() 
    {
        if (_screenStack.Count <= 1) return; 
        _screenStack.Pop().Free(); 

        _screenStack.Peek().Activate(); 
    }

    public void Push(EnumScreenName screen) 
    {
        if (_screenStack.Count > 0) _screenStack.Peek().Deactivate(); 
        _screenStack.Push(_dictionaryScreens[screen]); 
        _dictionaryScreens[screen].Activate(); 
    }
}

[System.Serializable]
public struct StructScreens
{
    public EnumScreenName screenName;
    public Transform screenObject;
}

public interface IScreenManager
{
    void Pop();
    void Push(EnumScreenName screen);
}
