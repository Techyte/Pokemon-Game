using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AllMoves))]
public class AllMovesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AllMoves allMoves = (AllMoves) target;

        if(GUILayout.Button("Add Move"))
        {
            Debug.Log(allMoves.moves.Keys.Count);

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
