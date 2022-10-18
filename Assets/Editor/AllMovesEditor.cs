#if UNITY_EDITOR
using PokemonGame.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace PokemonGame.Editor
{
    [CustomEditor(typeof(AllMoves))]
    public class AllMovesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllMoves allMoves = (AllMoves)target;

            if (GUILayout.Button("Add Move"))
            {
                allMoves.AddMove(allMoves.moveToAdd);
                
                allMoves.moveToAdd = null;
            }
            foreach (var p in allMoves.moves)
            {
                EditorGUILayout.LabelField(p.Key + ": " + p.Value);
            }
        }
    }
}
#endif