using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Camera cam;
    public Transform ptransform;
    public float sensitivity = 100f;
    private float rotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        float mousex = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        ptransform.RotateAround(ptransform.position, Vector3.up, mousex * sensitivity * Time.deltaTime);
        rotation -= mousey * Time.deltaTime * sensitivity;
        rotation = Mathf.Clamp(rotation, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);

    }
}
