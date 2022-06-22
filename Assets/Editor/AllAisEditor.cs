using UnityEditor;
using UnityEngine;

namespace PokemonGame
{
    [CustomEditor(typeof(AllAis))]
    public class AllAisEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllAis allAis = (AllAis)target;

            if (GUILayout.Button("Add AI"))
            {
                allAis.ais.Add(allAis.aiToAdd.name, allAis.aiToAdd);
            }
            if (Application.isPlaying)
            {
                foreach (var p in allAis.ais)
                {
                    EditorGUILayout.LabelField(p.Key + ": " + p.Value);
                }
            }
        }
    }
}