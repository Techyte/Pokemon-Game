namespace PokemonGame.Game.Trainers
{
    using UnityEngine;

    public class BattleStarterPlayerDetector : MonoBehaviour
    {
        private Trainer _trainer;

        private void Start()
        {
            _trainer = transform.parent.GetComponent<Trainer>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _trainer.StartBattleStartSequence();
            }
        }
    }   
}
