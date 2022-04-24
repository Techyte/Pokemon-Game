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
            Debug.Log("We pressed add move");
        }
    }
}
