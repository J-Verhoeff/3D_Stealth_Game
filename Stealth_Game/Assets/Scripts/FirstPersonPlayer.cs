using UnityEngine;

// Script to control movement of the first person player
[RequireComponent(typeof(CharacterController))]
public class FirstPersonPlayer : MonoBehaviour {
    [Header("General")]
    [SerializeField] private float movementSpeed = 15f;
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float defaultCameraHeight = 2f;

    [Header("Falling")]
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jumping")]
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private bool canAirControl = true;

    [Header("Looking")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSenitivity;
    [SerializeField] private float verticalClamp = 90f;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private float verticalSpeed = 0f;
    private bool isGrounded = false;
    private bool isCrouched = false;

    private void Awake() {
        controller = GetComponent<CharacterController>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        Vector3 x = Vector3.zero;
        Vector3 y = Vector3.zero;
        Vector3 z = Vector3.zero;

        // are we on the ground?
        RaycastHit collision;
        if(Physics.Raycast(groundPosition.position, Vector3.down, out collision, 0.2f, groundLayer)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        // update vertical speed
        if(!isGrounded) {
            verticalSpeed += gravityFactor * -9.81f * Time.deltaTime;
        } else {
            verticalSpeed = 0f;
        }

        // adjust the rotations based on mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSenitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSenitivity * Time.deltaTime;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -1f * verticalClamp, verticalClamp);
        playerCamera.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);


        // handle jumping
        if(isGrounded && Input.GetButtonDown("Jump")) {
            verticalSpeed = jumpSpeed;
            isGrounded = false;
            y = transform.up * verticalSpeed;
        } else if(!isGrounded){
            y = transform.up * verticalSpeed;
        }        

        // handle motion
        if (isGrounded || canAirControl) {
            x = transform.right * Input.GetAxis("Horizontal") * movementSpeed;
            z = transform.forward * Input.GetAxis("Vertical") * movementSpeed;
        }

        Vector3 movement = (x + y + z) * Time.deltaTime;
        controller.Move(movement);

        // handle crouching
        if(Input.GetKeyDown(KeyCode.C)) {
            if(!isCrouched) {
                playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, crouchHeight, playerCamera.localPosition.z);
                isCrouched = true;
                movementSpeed /= 2f;
            } else {
                playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, defaultCameraHeight, playerCamera.localPosition.z);
                isCrouched = false;
                movementSpeed *= 2f;
            }
        }
    }
}
