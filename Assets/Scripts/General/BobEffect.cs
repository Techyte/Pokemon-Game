using UnityEngine;

namespace PokemonGame
{
    public class BobEffect : MonoBehaviour
    {
        public float bobIntensity;
        public float bobSpeed;
        float _orignY;

        private void Start()
        {
            _orignY = transform.position.y;
        }

        private void Update()
        {
            Vector3 newPos = transform.position;
            newPos.y = Mathf.Sin(Time.time * bobSpeed) * bobIntensity;
            newPos.y += _orignY;

            transform.position = newPos;
        }
    }

}