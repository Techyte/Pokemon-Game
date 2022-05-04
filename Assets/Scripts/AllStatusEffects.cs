using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu]
public class AllStatusEffects : ScriptableObject, ISerializationCallbackReceiver
{
    public List<string> _keys = new List<string>();
    public List<StatusEffect> _values = new List<StatusEffect>();

    public Dictionary<string, StatusEffect> effects = new Dictionary<string, StatusEffect>();

    public StatusEffect MoveToAdd;

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in effects)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        effects = new Dictionary<string, StatusEffect>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            effects.Add(_keys[i], _values[i]);
    }
}
