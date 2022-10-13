using UnityEditor;
using UnityEngine;
using PokemonGame.ScriptableObjects;

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
                if(!AllAis.ais.TryGetValue(allAis.aiToAdd.name, out EnemyAI ai))
                {
                    AllAis.ais.Add(allAis.aiToAdd.name, allAis.aiToAdd);
                }
                else
                {
                    Debug.LogWarning("Item is already in the list, please do not try and add it again");
                }
                allAis.aiToAdd = null;
            }
            if (Application.isPlaying)
            {
                foreach (var p in AllAis.ais)
                {
                    EditorGUILayout.LabelField(p.Key + ": " + p.Value);
                }
            }
        }
    }
}