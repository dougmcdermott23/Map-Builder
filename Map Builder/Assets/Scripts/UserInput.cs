using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class UserInput : MonoBehaviour
{

    Controller controller;

    public Transform playerCamera;

    public float translateSensitivity;
    public float rotateSensitivity;
    public float telescopeSensitivity;

    float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<Controller>();
    }

    void Update()
    {
        Vector3 movement = new Vector3();

        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        // Translate Camera
        if (Input.GetMouseButton(0))
        {
            float translateHorizontal = horizontal * translateSensitivity * Time.deltaTime;
            float translateVertical = vertical * translateSensitivity * Time.deltaTime;

            movement -= playerCamera.TransformDirection(Vector3.right) * translateHorizontal + playerCamera.TransformDirection(Vector3.up) * translateVertical;
        }
        // Rotate Camera
        else if (Input.GetMouseButton(1))
        {
            float rotateHorizontal = horizontal * rotateSensitivity * Time.deltaTime;
            float rotateVertical = vertical * rotateSensitivity * Time.deltaTime;

            xRotation -= rotateVertical;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * rotateHorizontal);
        }

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            float telescope = Input.mouseScrollDelta.y * telescopeSensitivity;

            movement += playerCamera.TransformDirection(Vector3.forward) * telescope;
        }

        controller.Move(movement);

        if (Input.GetKey(KeyCode.A))
        {
            controller.SelectQuad();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            controller.UnselectQuad();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            controller.MoveVertex(vertical);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            controller.SetVertex();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            controller.UnsetVertex();
        }
    }
}
