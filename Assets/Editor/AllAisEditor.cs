#if UNITY_EDITOR
using PokemonGame.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace PokemonGame.Editor
{
    [CustomEditor(typeof(AllAis))]
    public class AllAisEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllAis allAis = (AllAis)target;

            if (GUILayout.Button("Add AI"))
            {
                allAis.AddMove(allAis.aiToAdd);
                
                allAis.aiToAdd = null;
            }
            foreach (var p in allAis.ais)
            {
                EditorGUILayout.LabelField(p.Key + ": " + p.Value);
            }
        }
    }
}
#endif