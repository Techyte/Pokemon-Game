using PokemonGame.Game;
using UnityEngine;

public class BattleStarterPlayerDetector : MonoBehaviour
{
    private Trainer trainer;

    private void Start()
    {
        trainer = transform.parent.GetComponent<Trainer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trainer.StartBattleSequence(other.GetComponent<Player>());
        }
    }
}
