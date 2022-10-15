using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static int seenClues = 0;

    public Transform clueSpot;

    private Outline outline;
    private bool isOutlined;

    private Vector3 startPos;
    private Quaternion startRot;
    private bool atStart = true;
    private bool clueSeen = false;
    private float lerpTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        gameObject.layer = LayerMask.NameToLayer("Item");

        //gives object outline for interaction clue
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineHidden;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 10f;
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
                if(Input.GetMouseButtonDown(1)){
                    seenClues++;
                    clueSeen = true;
                    kickOut();
                    Return();
                }
            }
        }

        if(isOutlined){
            outline.OutlineMode = Outline.Mode.OutlineAll;
        }
        else{
            outline.OutlineMode = Outline.Mode.OutlineHidden;
        }
        isOutlined = false;
    }

    public void Pickup(){
        if(!clueSeen){
            outline.OutlineMode = Outline.Mode.OutlineHidden;
            atStart = false;
            lerpTime = 0f;
        }
        else{
            kickOut();
        }
    }

    public void Return(){
        atStart = true;
        lerpTime = 0f;
    }

    public void DoOutline(){
        if(!clueSeen){
            isOutlined = true;
        }
    }

    private void kickOut(){
        Player.holding = false;
        Player.heldItem = null;
    }
}
