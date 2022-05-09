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

            if (GUILayout.Button("Add Status Effect"))
            {
                allAis.ais.Add(allAis.AIToAdd.name, allAis.AIToAdd);
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