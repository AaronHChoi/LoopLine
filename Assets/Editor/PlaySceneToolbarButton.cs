using Mono.Cecil;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

[InitializeOnLoad]
public static class PlaySceneToolbarButton
{
    private const string ScenePath = "Assets/Scenes/04. Train.unity";
    private const string MainMenuScenePath = "Assets/Scenes/01. MainMenu.unity";

    static PlaySceneToolbarButton()
    {
        EditorApplication.delayCall += AddButtonToToolbar;
    }

    private static void AddButtonToToolbar()
    {
        var toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        if (toolbarType == null) return;

        var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);
        if (toolbars.Length == 0)
        {
            EditorApplication.delayCall += AddButtonToToolbar;
            return;
        }

        var toolbar = (VisualElement)toolbarType
            .GetField("m_Root", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(toolbars[0]);

        if (toolbar == null)
        {
            EditorApplication.delayCall += AddButtonToToolbar;
            return;
        }

        VisualElement centerZone =
            toolbar.Q("ToolbarZonePlayMode") ??
            toolbar.Q("ToolbarZoneMiddleAlign") ??
            toolbar;

        if (centerZone.Q("CustomSceneButtonsContainer") != null) return;

        var container = new VisualElement
        {
            name = "CustomSceneButtonContainer",
            style =
            {
                flexDirection = FlexDirection.Row,
                alignSelf = Align.Center
            }
        };

        var btnMainMenu = CreateToolbarButton("▶ Main Menu", "Play Main Menu scene", new Color(0.3f, 0.5f, 0.7f, 1f), () => OnPlaySceneClicked(MainMenuScenePath));

        var btnTrain = CreateToolbarButton("▶ Train", "Play Train scene", new Color(0.4f, 0.65f, 0.4f, 1f), () => OnPlaySceneClicked(ScenePath));

        container.Add(btnMainMenu);
        container.Add(btnTrain);

        centerZone.Insert(0, container);
    }

    private static Button CreateToolbarButton(string text, string tooltip, Color bgColor, System.Action onClick)
    {
        var button = new Button(onClick)
        {
            text = text,
            tooltip = tooltip
        };

        button.style.backgroundColor = bgColor;
        button.style.color = Color.white;
        button.style.unityFontStyleAndWeight = FontStyle.Bold;
        button.style.height = 22;
        button.style.width = 100; // fixed width to avoid stretching
        button.style.borderTopLeftRadius = 4;
        button.style.borderTopRightRadius = 4;
        button.style.borderBottomLeftRadius = 4;
        button.style.borderBottomRightRadius = 4;
        button.style.alignSelf = Align.Center;
        button.style.justifyContent = Justify.Center;
        button.style.marginLeft = 6;
        button.style.marginRight = 6;
        button.style.paddingTop = 2;

        // Remove internal toolbar button styles that force white background
        button.RemoveFromClassList("unity-button");

        return button;
    }

    private static void OnPlaySceneClicked(string scenePath)
    {
        if (EditorApplication.isPlaying)
        {
            Debug.Log("Already in Play Mode.");
            return;
        }

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
            EditorApplication.EnterPlaymode();
        }
    }
}