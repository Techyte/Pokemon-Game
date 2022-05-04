using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu]
public class AllAis : ScriptableObject, ISerializationCallbackReceiver
{
    public List<string> _keys = new List<string>();
    public List<EnemyAI> _values = new List<EnemyAI>();

    public Dictionary<string, EnemyAI> ais = new Dictionary<string, EnemyAI>();

    public EnemyAI MoveToAdd;

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in ais)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        ais = new Dictionary<string, EnemyAI>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            ais.Add(_keys[i], _values[i]);
    }
}
