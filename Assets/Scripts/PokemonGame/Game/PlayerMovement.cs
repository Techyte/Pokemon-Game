using Cinemachine;

namespace PokemonGame.Game
{
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        [Header("Assigning")]
        public CharacterController controller;
        public CinemachineFreeLook camFreeLook;
        public Transform cam;
        public Transform groundDetectorPos;

        private Player _player;

        [Header("Control Values")]
        public float normalSpeed = 6f;

        private float _turnSmoothTime = 0.1f;
        private float _turnSmoothVelocity;
        public bool canMove = true;
        [SerializeField] float speed;
        [SerializeField] private LayerMask ground;

        private int _frameSkips = 0;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            speed = normalSpeed;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (_frameSkips >= 2)
            {
                if (canMove && !_player.interacting)
                {
                    camFreeLook.enabled = true;
                    
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                
                    float horizontal = Input.GetAxis("Horizontal");
                    float vertical = Input.GetAxis("Vertical");
                    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
                
                    if (direction.magnitude >= 0.1f)
                    {
                        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    
                        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        controller.Move( speed * Time.deltaTime * moveDirection.normalized);
                    }
                }
                else
                {
                    camFreeLook.enabled = false;
                    
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            else
            {
                _frameSkips++;
            }
            
            if (Physics.Raycast(groundDetectorPos.position, Vector3.down, out RaycastHit hit, 10f, ground))
            {
                Vector3 newPos = transform.position;
                newPos.y = hit.point.y+transform.localScale.y/2;
                transform.position = newPos;
            }
        }
    }
}