using System;
using System.Collections.Generic;
using PokemonGame.Battle;
using PokemonGame.ScriptableObjects;
using UnityEditorInternal;
using UnityEngine;

namespace PokemonGame.Editor
{
    using UnityEditor;
    
    [CustomEditor(typeof(BattleSequence))]
    public class BattleSequenceEditor : Editor
    {
        SerializedProperty sequence;
        
        ReorderableList list;

        private void OnEnable()
        {
            sequence = serializedObject.FindProperty("sequence");

            // Set up the reorderable list       
            list = new ReorderableList(serializedObject, sequence, true, true, true, true);
            list.drawElementCallback = DrawListItems; // Delegate to draw the elements on the list
            list.drawHeaderCallback = DrawHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
            list.onAddCallback = OnAdd;
        }

        private void OnAdd(ReorderableList reorderableList)
        {
            
        }

        // Draws the elements on the list
        void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            BattleEvent battleEvent = ((BattleSequence)target).sequence[index];
            
            try
            {
                Moves moves = (Moves)battleEvent;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), battleEvent.Name);

                moves.nestedMoveEvents = (BattleSequence)EditorGUI.ObjectField(
                    new Rect(rect.x + 50, rect.y, rect.width-50, EditorGUIUtility.singleLineHeight), moves.nestedMoveEvents,
                    typeof(BattleSequence), false);

                // Save changes to the object
                if (GUI.changed) {
                    EditorUtility.SetDirty((BattleSequence)target);
                }
            }
            catch (Exception e)
            {
                // not a moves event
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), battleEvent.Name);
            }
        }

        //Draws the header
        void DrawHeader(Rect rect)
        {
            string name = "Battle Sequence";
            EditorGUI.LabelField(rect, name);
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            // 1. Get a reference to the target script
            BattleSequence script = (BattleSequence)target;

            list.DoLayoutList(); // Have the ReorderableList do its work

            // We need to call this so that changes on the Inspector are saved by Unity.
            
            // 2. Store the current value before drawing the popup
            BattleSequence.BattleEventsToAdd previousValue = script.eventToAdd;
            
            // 3. Draw the dropdown
            script.eventToAdd = (BattleSequence.BattleEventsToAdd)EditorGUILayout.EnumPopup("", script.eventToAdd);
            
            // 4. If the value changed, execute your logic
            if (script.eventToAdd != previousValue)
            {
                Debug.Log("Option changed to: " + script.eventToAdd);
                script.AddBattleEvent(); // Call the method in your main script
                script.eventToAdd = 0;
            }
            
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Clear"))
            {
                script.sequence = new List<BattleEvent>();
            }
        }
    }
}