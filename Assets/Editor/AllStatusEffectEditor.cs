using UnityEditor;
using UnityEngine;

namespace PokemonGame.Battle
{
    [CustomEditor(typeof(AllStatusEffects))]
    public class AllStatusEffectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllStatusEffects allMoves = (AllStatusEffects)target;

            if (GUILayout.Button("Add Status Effect"))
            {
                Debug.Log(allMoves.effects.Keys.Count);

                allMoves.effects.Add(allMoves.MoveToAdd.name, allMoves.MoveToAdd);
            }
            if (Application.isPlaying)
            {
                foreach (var p in allMoves.effects)
                {
                    EditorGUILayout.LabelField(p.Key + ": " + p.Value);
                }
            }
        }
    }

}