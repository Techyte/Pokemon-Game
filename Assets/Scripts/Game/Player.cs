using UnityEngine;

namespace PokemonGame.Game
{
    public class Player : MonoBehaviour
    {
        public void LookAtTrainer(Vector3 trainerPos)
        {
            Quaternion currentRotation = transform.rotation;
            transform.LookAt(new Vector3(trainerPos.x, transform.position.y, trainerPos.z));
            Quaternion targetRotation = transform.rotation;
            transform.rotation = currentRotation;
            targetRotation.x = 0;
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 0.04f);
        }
    }
}