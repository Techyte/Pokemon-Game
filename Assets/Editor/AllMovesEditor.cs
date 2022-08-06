using UnityEditor;
using UnityEngine;
using PokemonGame.ScriptableObjects;

namespace PokemonGame
{
    [CustomEditor(typeof(AllMoves))]
    public class AllMovesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AllMoves allMoves = (AllMoves)target;

            if (GUILayout.Button("Add Move"))
            {
                allMoves.moves.Add(allMoves.moveToAdd.name, allMoves.moveToAdd);
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