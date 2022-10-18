#if UNITY_EDITOR
using PokemonGame.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace PokemonGame.Editor
{
    [CustomEditor(typeof(AllStatusEffects))]
    public class AllStatusEffectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllStatusEffects allEffects = (AllStatusEffects)target;

            if (GUILayout.Button("Add Status Effect"))
            {
                allEffects.AddEffect(allEffects.effectToAdd);
                
                allEffects.effectToAdd = null;
            }
            foreach (var p in allEffects.effects)
            {
                EditorGUILayout.LabelField(p.Key + ": " + p.Value);
            }
        }
    }

}
#endif