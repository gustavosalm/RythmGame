using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNoteHit : MonoBehaviour
{
    [SerializeField] private GameObject rc;
    [SerializeField] private GM gm;
    [SerializeField] private Color32[] squareColors;
    private GameObject enemyObject;
    void Start()
    {
        enemyObject = transform.GetChild(0).gameObject;
        enemyObject.SetActive(false);
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
        if(Input.GetKeyDown(KeyCode.Space))
            PlayAnim();
    }

    public void PlayAnim(){
        enemyObject.SetActive(true);
        enemyObject.GetComponent<Animator>().Play("CarrinhoAnim", 0, 0);
        StartCoroutine("StopAnim");
    }
    public IEnumerator StopAnim(){
        yield return new WaitForSeconds(1);
        enemyObject.SetActive(false);
    }

    public void MovePlayer(int noteHit){
        if(noteHit > 1 && (gm.scene == "PR" || gm.scene == "RR"))
            return;
        
        if(!gameObject.activeSelf && gm.scene != "TACKLE")
            gameObject.SetActive(true);

        switch(gm.scene){
            case "PR":
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position - new Vector3(.7f, 0, 0);
                gameObject.GetComponent<SpriteRenderer>().color = squareColors[0];
                // transform.position = Vector3.MoveTowards(transform.position, rc.transform.GetChild(noteHit).GetChild(2).position, 2 * Time.deltaTime);
                break;
            case "RR":
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position + new Vector3(.7f, 0, 0);
                gameObject.GetComponent<SpriteRenderer>().color = squareColors[1];
                // transform.position = Vector3.MoveTowards(transform.position, rc.transform.GetChild(noteHit).GetChild(2).position, 2 * Time.deltaTime);
                break;
            case "TACKLE":
                gameObject.SetActive(false);
                break;
            case "RGK":
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position + new Vector3(0, 1, 0);
                gameObject.GetComponent<SpriteRenderer>().color = squareColors[1];
                break;
            case "PGK":
                transform.position = rc.transform.GetChild(noteHit).GetChild(2).position - new Vector3(0, 1, 0);
                gameObject.GetComponent<SpriteRenderer>().color = squareColors[0];
                break;
        }
    }
}
