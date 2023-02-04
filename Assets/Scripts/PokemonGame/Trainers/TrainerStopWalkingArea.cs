namespace PokemonGame.Game.Trainers
{
    using UnityEngine;

    public class TrainerStopWalkingArea : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Trainer trainer = other.GetComponent<Trainer>();

            if (trainer)
            {
                trainer.StartBattle();
            }
        }
    }
}

