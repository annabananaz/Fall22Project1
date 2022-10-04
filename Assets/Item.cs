using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform clueSpot;

    private Vector3 startPos;
    private Quaternion startRot;
    private bool atStart = true;
    private float lerpTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    void Update()
    {
        if(lerpTime <= 1f){
            if(atStart){
                transform.position = Vector3.Lerp(transform.position, startPos, lerpTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, startRot, lerpTime);
            }
            else{
                transform.position = Vector3.Lerp(transform.position, Player.player.hand.position, lerpTime);
            }
            lerpTime += Time.deltaTime;
        }

        if(!atStart){
            RaycastHit hit;
            if(Physics.Raycast(clueSpot.position, clueSpot.position - transform.position, out hit, 1f, 1 << LayerMask.NameToLayer("Player"))){
                Player.player.seeClue = true;
                print("see the thing");
            }
        }
    }

    public void Pickup(){
        atStart = false;
        lerpTime = 0f;
    }

    public void Return(){
        atStart = true;
        lerpTime = 0f;
    }
}
