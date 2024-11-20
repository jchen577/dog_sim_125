using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement: MonoBehaviour {
    // Start is called before the first frame update
    public float speed = 10f;
    public float rotationSpeed = 1f;
    public float XRotation;
    public float YRotation;
    public float gravity = -9.81f;
    [SerializeField] private float _pushForce = 50f;

    private CharacterController character;
    void Start() {
        character = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        if (character != null) {
            if (Input.GetKeyDown(KeyCode.Space)) {//Camera Lock
                if (Cursor.lockState == CursorLockMode.None) {
                    Cursor.lockState = CursorLockMode.Locked;
                } else {
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            //Camera rotation
            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
            YRotation -= YaxisRotation;
            YRotation = Mathf.Clamp(YRotation, -40f, 40f);
            XRotation -= XaxisRotation;
            transform.localRotation = Quaternion.Euler(YRotation, -XRotation, 0f);

            //Player movement/physics
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 grav;
            if (character.isGrounded) {
                grav = new Vector3(0, 0, 0);
            } else {
                grav = new Vector3(0, gravity, 0) / speed;
            }
            character.Move((transform.forward * moveZ + grav + transform.right * moveX) * speed * Time.deltaTime);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit){
    Rigidbody rb = hit.collider.attachedRigidbody;
    if(rb!=null && !rb.isKinematic){
        rb.AddForce(hit.moveDirection*_pushForce);
    }
}
}
