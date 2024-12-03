using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerMovement: MonoBehaviour {
    // Start is called before the first frame update
    public float speed = 10f;
    public float rotationSpeed = 1f;
    public float XRotation;
    public float YRotation;
    public float gravity = -9.81f;
    public float jumpHeight = 0.35f;
    public Vector3 jumpVelocity;
    public TMP_Text score;
    public int scoreNumber;
    public float boost;
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
            if (character.isGrounded && jumpVelocity.y < 0) {
                jumpVelocity.y = 0;
                boost = 0;
            }
            if (Input.GetKeyDown(KeyCode.C) && character.isGrounded) {
                jumpVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                boost = 5;
            }
            jumpVelocity.y += gravity * Time.deltaTime;
            character.Move(((transform.forward*(moveZ + boost)) + jumpVelocity + transform.right*moveX)*speed*Time.deltaTime);
            boost /= 1.05f;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit){
    Rigidbody rb = hit.collider.attachedRigidbody;
    if(rb!=null && !rb.isKinematic){
        rb.AddForce(hit.moveDirection*_pushForce);
    }
}
}
