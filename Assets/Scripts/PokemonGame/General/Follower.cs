using UnityEngine;
using PathCreation;

namespace PokemonGame
{
    public class Follower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public float speed = 5;
        float _distanceTravelled;
        public float xRotation;

        private void Update()
        {
            _distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled);
            transform.LookAt(Vector3.zero);
        }
    }

}