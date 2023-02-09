namespace PokemonGame.General
{
    using UnityEngine;

    public class Billboard : MonoBehaviour
    {
        public Transform cam;

        private void LateUpdate()
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}