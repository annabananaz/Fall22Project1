using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;

    public Rigidbody body;
    public Camera cam;
    public float sensitivity = 100f;

    private float rotation = 0f;

    public float playerSpeed = 2.0f;

    //used to track if player is holding/observing item
    public static bool holding = false;

    public Transform hand;
    public static Transform heldItem;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        player = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mousex = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        body.velocity = Vector3.zero;

        if(Input.GetMouseButtonDown(0)){

            if(holding){
                heldItem.SendMessage("Return");
                holding = false;
            }
            else{
                RaycastHit hit;
                if(Physics.Raycast(hand.position, hand.forward, out hit, 1f)){
                    if(hit.collider.tag == "Holdable"){
                        heldItem = hit.transform;
                        holding = true;
                        heldItem.SendMessage("Pickup");
                    }
                }
            }
        }

        if(holding){
            heldItem.RotateAround(heldItem.position, transform.forward, -mousex * sensitivity * Time.deltaTime);
            heldItem.RotateAround(heldItem.position, transform.right, mousey * sensitivity * Time.deltaTime);
        }
        else{
            transform.RotateAround(transform.position, Vector3.up, mousex * sensitivity * Time.deltaTime);
            rotation -= mousey * Time.deltaTime * sensitivity;
            rotation = Mathf.Clamp(rotation, -90f, 90f);
            cam.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
            

            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            move = move.normalized;

            body.velocity = (move.x * transform.right + move.z * transform.forward) * Time.deltaTime * playerSpeed;

            RaycastHit hit;
            if(Physics.Raycast(hand.position, hand.forward, out hit, 1f, 1 << LayerMask.NameToLayer("Interactable"))){
                hit.transform.SendMessage("DoOutline");
            }
        }
    }
}
