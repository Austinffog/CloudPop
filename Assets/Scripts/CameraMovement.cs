using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.cloudpop
{

    public class CameraMovement : MonoBehaviour
    {
        public float mouseSensitivity = 100f;
        public Transform playerBody;
        float xRotation = 0f;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = true; //dont hide mouse cursor
            //Cursor.lockState = CursorLockMode.Locked; //prevents cursor going off screen
            playerBody = GameObject.Find("Player(Clone)").GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Pause.paused) return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY; //prevents rotation flipping
            xRotation = Mathf.Clamp(xRotation, -45f, 90f); //clamp rotation of camera
            transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

        }
    }
}
