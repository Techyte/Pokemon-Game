using System;
using PokemonGame.Game;
using UnityEngine;

public class BattleStarterPlayerDetector : MonoBehaviour
{
    private BattleStarter _starter;

    private void Start()
    {
        _starter = transform.parent.GetComponent<BattleStarter>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _starter.StartBattleSequence(other.GetComponent<Player>());
        }
    }
}
