using UnityEditor;
using UnityEngine;

namespace PokemonGame
{
    [CustomEditor(typeof(AllStatusEffects))]
    public class AllStatusEffectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllStatusEffects allEffects = (AllStatusEffects)target;

            if (GUILayout.Button("Add Status Effect"))
            {
                AllStatusEffects.effects.Add(allEffects.effectToAdd.name, allEffects.effectToAdd);
            }
            if (Application.isPlaying)
            {
                foreach (var p in AllStatusEffects.effects)
                {
                    EditorGUILayout.LabelField(p.Key + ": " + p.Value);
                }
            }
        }
    }

}