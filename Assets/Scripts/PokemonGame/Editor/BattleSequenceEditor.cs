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
            
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), battleEvent.Name());

            if (CanCast<Moves>(battleEvent))
            {
                Moves moves = (Moves)battleEvent;

                moves.nestedMoveEvents = (BattleSequence)EditorGUI.ObjectField(
                    new Rect(rect.x + 50, rect.y, rect.width-50, EditorGUIUtility.singleLineHeight), moves.nestedMoveEvents,
                    typeof(BattleSequence), false);
            }
        }

        void DrawHeader(Rect rect)
        {
            string name = "Battle Sequence";
            EditorGUI.LabelField(rect, name);
        }
        
        public override void OnInspectorGUI()
        {
            EditorUtility.SetDirty(target);
            serializedObject.Update();

            list.DoLayoutList();
            
            BattleSequence script = (BattleSequence)target;
            BattleSequence.BattleEventsToAdd previousValue = script.eventToAdd;
            
            script.eventToAdd = (BattleSequence.BattleEventsToAdd)EditorGUILayout.EnumPopup("", script.eventToAdd);
            
            if (script.eventToAdd != previousValue)
            {
                Debug.Log("Option changed to: " + script.eventToAdd);
                script.AddBattleEvent();
                script.eventToAdd = 0;
            }

            if (GUILayout.Button("Clear"))
            {
                script.sequence = new List<BattleEvent>();
            }
        }

        private bool CanCast<T>(object obj)
        {
            try
            {
                T cast = (T)obj;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}