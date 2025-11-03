using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

[InitializeOnLoad]
public static class PlaySceneToolbarButton
{
    private const string ScenePath = "Assets/Scenes/04. Train.unity";

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

        // Create the button
        var button = new Button(OnPlaySceneClicked)
        {
            text = "  ▶ Train Scene",
            tooltip = "Play the main scene"
        };
        // Force the button to show a green background
        button.style.backgroundColor = new Color(0.4f, 0.65f, 0.4f, 1f); // Solid green
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

        centerZone.Insert(0, button);
    }

    private static void OnPlaySceneClicked()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.Log("Already in Play Mode.");
            return;
        }

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(ScenePath);
            EditorApplication.EnterPlaymode();
        }
    }
}
