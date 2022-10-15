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
                if(!AllMoves.moves.TryGetValue(allMoves.moveToAdd.name, out Move move))
                {
                    AllMoves.moves.Add(allMoves.moveToAdd.name, allMoves.moveToAdd);
                }
                else
                {
                    Debug.LogWarning("Item is already in the list, please do not try and add it again");
                }
                allMoves.moveToAdd = null;
            }
            if (Application.isPlaying)
            {
                foreach (var p in AllMoves.moves)
                {
                    EditorGUILayout.LabelField(p.Key + ": " + p.Value);
                }
            }
        }
    }
}
#endif