using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;

    public CharacterController controller;
    public Camera cam;
    public float sensitivity = 100f;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float rotation = 0f;

    public float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

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
    void Update()
    {
        float mousex = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        if(Input.GetMouseButtonDown(0)){

            if(holding){
                heldItem.SendMessage("Return");
                holding = false;
            }
            else{
                RaycastHit hit;
                if(Physics.Raycast(hand.position, hand.forward, out hit, 5f, 1 << LayerMask.NameToLayer("Item"))){
                    heldItem = hit.transform;
                    holding = true;
                    heldItem.SendMessage("Pickup");
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
            
            //player movement
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
            playerVelocity.y += gravityValue * Time.deltaTime;

            Vector3 move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
            move = move.normalized;

            playerVelocity.x = move.x;
            playerVelocity.z = move.z;

            controller.Move(playerVelocity * Time.deltaTime);

            RaycastHit hit;
            if(Physics.Raycast(hand.position, hand.forward, out hit, 5f, 1 << LayerMask.NameToLayer("Item"))){
                hit.transform.SendMessage("DoOutline");
            }
        }
    }
}
