#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PokemonGame.ScriptableObjects
{
    [CustomEditor(typeof(AllItems))]
    public class AllItemsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllItems allItems = (AllItems)target;

            if (GUILayout.Button("Add Item"))
            {
                allItems.AddItem(allItems.itemToAdd);
                
                allItems.itemToAdd = null;
            }
            foreach (var p in allItems.items)
            {
                EditorGUILayout.LabelField(p.Key + ": " + p.Value);
            }
        }
    }
}
#endif