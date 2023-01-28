using UnityEngine;

namespace PokemonGame.Game
{
    public class Player : MonoBehaviour
    {
        public Quaternion targetRot;
        
        public void LookAtTrainer(Vector3 trainerPos)
        {
            transform.LookAt(new Vector3(trainerPos.x, transform.position.y, trainerPos.z));
            Quaternion targetRotation = transform.rotation;
            targetRotation.x = 0;
            targetRot = targetRotation;
        }

        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.04f * Time.fixedDeltaTime);
        }
    }
}