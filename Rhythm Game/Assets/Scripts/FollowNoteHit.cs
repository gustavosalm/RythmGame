using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNoteHit : MonoBehaviour
{
    [SerializeField] private GameObject rc;
    [SerializeField] private GM gm;
    private Vector3 behind;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            MovePlayer(0);
        else if(Input.GetKeyDown(KeyCode.W))
            MovePlayer(1);
        else if(Input.GetKeyDown(KeyCode.E))
            MovePlayer(2);
        else if(Input.GetKeyDown(KeyCode.R))
            MovePlayer(3);

        // transform.position = rc.transform.GetChild(0).GetChild(2).position;
    }

    public void MovePlayer(int noteHit){
        if(noteHit > 1 && (gm.scene == "PR" || gm.scene == "RR"))
            return;

        switch(gm.scene){
            case "PR":
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position - new Vector3(.7f, 0, 0);
                // transform.position = Vector3.MoveTowards(transform.position, rc.transform.GetChild(noteHit).GetChild(2).position, 2 * Time.deltaTime);
                break;
            case "RR":
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position + new Vector3(.7f, 0, 0);
                // transform.position = Vector3.MoveTowards(transform.position, rc.transform.GetChild(noteHit).GetChild(2).position, 2 * Time.deltaTime);
                break;
            case "TACKLE":
                float mod = (noteHit % 2) * -1.4f;
                print(mod);
                print(mod + 0.7f);
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position + new Vector3(mod + .7f, 0, 0);
                break;
            case "RGK":
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position + new Vector3(0, 1, 0);
                break;
            case "PGK":
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position - new Vector3(0, 1, 0);
                break;
        }
    }
}
