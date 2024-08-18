namespace PokemonGame.General
{
    using UnityEngine;

    public class Billboard : MonoBehaviour
    {
        public Transform cam;

        private void Awake()
        {
            if (!cam)
            {
                cam = Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}