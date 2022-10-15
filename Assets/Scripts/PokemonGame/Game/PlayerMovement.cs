using UnityEngine;
using PokemonGame.Dialogue;

namespace PokemonGame.Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Assigning")]
        public CharacterController controller;
        public Transform cam;
        public Transform groundDetectorPos;

        [Header("Control Values")]
        public float normalSpeed = 6f;

        private float turnSmoothTime = 0.1f;
        private float _turnSmoothVelocity;
        public bool canMove = true;
        [SerializeField] float speed;
        public bool battleStarterHasStartedWalking;
        [SerializeField] private LayerMask ground;
        
        private void Start()
        {
            speed = normalSpeed;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (battleStarterHasStartedWalking)
            {
                canMove = false;
            }
            
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
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move( speed * Time.deltaTime * moveDirection.normalized);
                }
            }

            if (Physics.Raycast(groundDetectorPos.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Infinity, ground))
            {
                Vector3 newPos = transform.position;
                newPos.y = hit.point.y+transform.localScale.y/2;
                transform.position = newPos;
            }
        }
    }
}