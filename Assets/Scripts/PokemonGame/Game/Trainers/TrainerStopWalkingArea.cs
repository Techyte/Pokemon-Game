using PokemonGame.Game.Trainers;
using UnityEngine;

public class TrainerStopWalkingArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trainer"))
        {
            Trainer trainer = other.GetComponent<Trainer>();
            trainer.StartBattle();
        }
    }
}
