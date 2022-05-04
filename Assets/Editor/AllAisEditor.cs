using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AllAis))]
public class AllAisEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AllAis allAis = (AllAis)target;

        if (GUILayout.Button("Add AI"))
        {
            Debug.Log(allAis.ais.Keys.Count);

            allAis.ais.Add(allAis.MoveToAdd.name, allAis.MoveToAdd);
        }
        if (Application.isPlaying)
        {
            foreach (var p in allAis.ais)
            {
                EditorGUILayout.LabelField(p.Key + ": " + p.Value);
            }
        }
    }
}
