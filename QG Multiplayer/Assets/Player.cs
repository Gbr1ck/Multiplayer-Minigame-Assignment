using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float ms = 10;
    public float jumpForce = 10;
    public float sprintMulty = 1.5f;
    private bool grounded = false;
    public GameObject cameraObj;
    private Vector3 cameraDelta = new Vector3 ();
    Rigidbody rb;

    void Start () => rb = this.GetComponent<Rigidbody> ();

    void Update () {
        if (Input.GetMouseButton (0) || Input.GetMouseButton (1) || Input.GetMouseButton (2)) Cursor.lockState = CursorLockMode.Locked;
        else if (Input.GetButtonDown ("Cancel")) Cursor.lockState = CursorLockMode.None;

        if (Cursor.lockState == CursorLockMode.Locked) {
            RaycastHit hit;
            // player rotation, based on camera
            cameraDelta += new Vector3 (-Input.GetAxis ("Mouse Y"), Input.GetAxis ("Mouse X"), 0);
            transform.rotation = Quaternion.Euler (0, cameraDelta.y, 0);

            // Jumping
            grounded = (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.down), out hit, this.GetComponent<SphereCollider> ().radius + 0.1f));
            if (Input.GetButtonDown ("Jump") && grounded) rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.VelocityChange);

            // Basic Movement
            Vector3 dir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
            dir.Normalize ();
            rb.AddRelativeForce (dir * ms * Time.deltaTime * (Input.GetButton ("Sprint") ? sprintMulty : 1), ForceMode.VelocityChange);

            // camera controls
            cameraObj.transform.rotation = Quaternion.Euler (cameraDelta);
        } else cameraObj.transform.rotation = Quaternion.Euler (cameraDelta);
    }
}