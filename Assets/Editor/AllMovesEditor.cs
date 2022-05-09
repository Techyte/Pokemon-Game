using UnityEditor;
using UnityEngine;

namespace PokemonGame
{
    [CustomEditor(typeof(AllMoves))]
    public class AllMovesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllMoves allMoves = (AllMoves)target;

            if (GUILayout.Button("Add Status Effect"))
            {
                allMoves.moves.Add(allMoves.MoveToAdd.name, allMoves.MoveToAdd);
            }
            if (Application.isPlaying)
            {
                foreach (var p in allMoves.moves)
                {
                    EditorGUILayout.LabelField(p.Key + ": " + p.Value);
                }
            }
        }
    }
}