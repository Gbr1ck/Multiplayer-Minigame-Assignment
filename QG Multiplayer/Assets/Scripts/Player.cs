using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks {
    public float ms = 10;
    public float jumpForce = 10;
    public float sprintMulty = 2f;
    bool grounded = false;
    public GameObject PlayerUIPrefab;
    public GameObject cameraObj;
    Camera mainCamera;
    public Camera playerCamera;
    Vector3 cameraDelta = new Vector3 ();
    public TextMeshPro name;
    Rigidbody rb;

    // dash
    public float dashCoolDown = 2f;
    public float dashDelta = 0.3f;
    public float dashForce = 1f;
    float[] dashDirection = { 0, 0, 0, 0, 0 };
    int dashing = -1;

    void Start () {
        rb = this.GetComponent<Rigidbody> ();
        //setting up the cameras and checking to make sure the current camera is set to the correct player
        mainCamera = Camera.main;
        playerCamera.enabled = false;
        name.text = this.photonView.Owner.NickName;
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        } else {
            mainCamera.enabled = false;
            playerCamera.enabled = true;
            Destroy (name.gameObject);
        }

        //setting up player UI
        if (PlayerUIPrefab != null) {
            GameObject UIGameObject = Instantiate (PlayerUIPrefab);
        }
    }

    public void CameraLookat (Transform _Camera) {
        name.transform.LookAt (_Camera, Vector3.up);
        name.transform.Rotate (new Vector3 (0, 180, 0));
    }

    void Update () {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            transform.rotation = new Quaternion ();
            return;
        }

        if (Input.GetMouseButton (0) || Input.GetMouseButton (1) || Input.GetMouseButton (2)) Cursor.lockState = CursorLockMode.Locked;
        else if (Input.GetButtonDown ("Cancel")) Cursor.lockState = CursorLockMode.None;

        if (Cursor.lockState == CursorLockMode.Locked) {
            // dash state update
            dashing = -1;
            if (Input.GetButtonDown ("Horizontal")) {
                if (dashDirection[(int) Input.GetAxisRaw ("Horizontal") + 1] > 0)
                    dashing = (int) Input.GetAxisRaw ("Horizontal") + 1;
                dashDirection = new float[5] { 0, 0, 0, 0, dashDirection[4] };
                dashDirection[(int) Input.GetAxisRaw ("Horizontal") + 1] = dashDelta;
            }
            if (Input.GetButtonDown ("Vertical")) {
                if (dashDirection[(int) Input.GetAxisRaw ("Vertical") + 2] > 0)
                    dashing = (int) Input.GetAxisRaw ("Vertical") + 2;
                dashDirection = new float[5] { 0, 0, 0, 0, dashDirection[4] };
                dashDirection[(int) Input.GetAxisRaw ("Vertical") + 2] = dashDelta;
            }

            RaycastHit hit;
            // player rotation, based on camera
            cameraDelta += new Vector3 (-Input.GetAxis ("Mouse Y"), Input.GetAxis ("Mouse X"), 0);
            transform.rotation = Quaternion.Euler (0, cameraDelta.y, 0);

            // Jumping
            grounded = (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.down), out hit, this.GetComponent<SphereCollider> ().radius + 0.1f));
            if (Input.GetButtonDown ("Jump") && grounded) rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.VelocityChange);

            // Dashing 
            if (dashing >= 0 && dashDirection[4] == 0 && Input.GetMouseButton (1)) {
                Dash (dashing);
                dashDirection[4] = dashCoolDown;
            } else { // Basic Movement
                Vector3 dir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
                dir.Normalize ();
                rb.AddRelativeForce (dir * ms * Time.deltaTime * (Input.GetButton ("Sprint") ? sprintMulty : 1), ForceMode.VelocityChange);
            }

            // camera controls
            cameraObj.transform.rotation = Quaternion.Euler (cameraDelta);
        } else cameraObj.transform.rotation = Quaternion.Euler (cameraDelta);

        // timer reset
        for (var d = 0; d < dashDirection.Length; d++) {
            if (dashDirection[d] > 0) {
                dashDirection[d] -= Time.deltaTime;
                if (dashDirection[d] < 0) dashDirection[d] = 0;
            }
        }
    }

    void LateUpdate () {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;
        foreach (Player p in FindObjectsOfType<Player> ())
            if (p != this) p.CameraLookat (playerCamera.transform);
    }

    void Dash (int direction) {
        Debug.Log ("DASH");
        switch (direction % 2) {
            case 0: // Horizontal
                rb.AddRelativeForce (-(1 - direction) * dashForce * Vector3.right * (Input.GetButton ("Sprint") ? sprintMulty : 1), ForceMode.VelocityChange);
                break;
            case 1: // Vertical
                rb.AddRelativeForce (-(2 - direction) * dashForce * Vector3.forward * (Input.GetButton ("Sprint") ? sprintMulty : 1), ForceMode.VelocityChange);
                break;
        }
    }
}