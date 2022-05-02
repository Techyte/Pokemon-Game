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
            allMoves.moves.Add(allMoves.MoveToAdd.name, allMoves.MoveToAdd);

            allMoves.myString.Add("Added lol");

            Debug.Log(allMoves.moves);
        }

        foreach(var p in allMoves.moves)
        {
            EditorGUILayout.LabelField(p.Key + ": " + p.Value);
        }
    }
}
