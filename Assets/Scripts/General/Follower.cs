using UnityEngine;
using PathCreation;

namespace PokemonGame
{
    public class Follower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public float speed = 5;
        float distanceTravelled;
        public float xRotation;

        private void Update()
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.LookAt(Vector3.zero);
        }
    }

}