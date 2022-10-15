using UnityEngine;
using UnityEditor;

namespace PokemonGame.Editor
{
public class PopupExample : PopupWindowContent
{
    bool _allItems;
    bool _allAis;
    bool _allMoves; 
    bool _allStatusEffects;

    private RegisterEditorWindow _window;
    public PopupExample(RegisterEditorWindow window)
    {
        _window = window;
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(200, 110);
    }

    public override void OnGUI(Rect rect)
    {
        GUILayout.Label("Select what to view", EditorStyles.boldLabel);
        _allItems = GUILayout.Button("All Items");
        _allAis = GUILayout.Button("All Ais");
        _allMoves = GUILayout.Button("All Moves");
        _allStatusEffects = GUILayout.Button("All Status Effects");

        if (_allItems)
        {
            Debug.Log("All Items Clicked");
            _window.OpenNewScreen(RegisterViewId.AllItems);
        }

        if (_allAis)
        {
            Debug.Log("All Ais Clicked");
            _window.OpenNewScreen(RegisterViewId.AllAis);
        }

        if (_allMoves)
        {
            Debug.Log("All Moves Clicked");
            _window.OpenNewScreen(RegisterViewId.AllMoves);
        }

        if (_allStatusEffects)
        {
            Debug.Log("All Status Effects Clicked");
            _window.OpenNewScreen(RegisterViewId.AllStatusEffects);
        }
    }
}   
}