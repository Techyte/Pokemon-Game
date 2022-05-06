using UnityEngine;

namespace PokemonGame.Game
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        [SerializeField] private float speed;

        private void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            /*
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");

            rb.velocity = new Vector3(xInput * speed, 0, yInput * speed);
            */

            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = Vector3.forward * speed;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = Vector3.left * speed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = Vector3.back * speed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = Vector3.right * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}