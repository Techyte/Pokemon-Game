using UnityEngine;
using PokemonGame.Dialogue;

namespace PokemonGame.Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Asignables")]
        public CharacterController controller;
        public Transform cam;

        [Header("Control Values")]
        public float normalSpeed = 6f;

        private float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;
        public bool canMove = true;
        [SerializeField] float speed;
        private void Start()
        {
            speed = normalSpeed;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            canMove = !DialogueManager.GetInstance().dialogueIsPlaying;
            if (canMove)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

                if (direction.magnitude >= 0.1f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDirection.normalized * speed * Time.deltaTime);
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}