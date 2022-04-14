using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNoteHit : MonoBehaviour
{
    [SerializeField] private GameObject rc, ball;
    [SerializeField] private GameObject[] scoreObjs;
    [SerializeField] private GM gm;
    [SerializeField] private Color32[] squareColors;
    [SerializeField] private GameObject[] anims;
    private SpriteRenderer sr;
    private GameObject enemyObject, enemyBall, player, rival;
    private Vector3 mod, anchor, plScale;
    public int time, lastHitNote;
    private int scaled = 1, tempo = 0;
    public bool ballTaken = false;

    void Start() {
        sr = gameObject.GetComponent<SpriteRenderer>();
        enemyObject = transform.GetChild(0).gameObject;
        enemyBall = enemyObject.transform.GetChild(1).gameObject;
        enemyBall.SetActive(false);
        enemyObject.SetActive(false);
        mod = new Vector3(0, 0, 0);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Q))
            MovePlayer(0);
        else if(Input.GetKeyDown(KeyCode.W))
            MovePlayer(1);
        else if(Input.GetKeyDown(KeyCode.E))
            MovePlayer(2);
        else if(Input.GetKeyDown(KeyCode.R))
            MovePlayer(3);

        // transform.position = rc.transform.GetChild(0).GetChild(2).position;
        // if(Input.GetKeyDown(KeyCode.Space))
        //     PlayAnim();
    }

    public void SetTeam(int a, int b){
        player = anims[a];
        rival = anims[b];
        rival.gameObject.tag = "enemy";
        player.SetActive(true);
        for(int i = 0; i < 4; i++)
            if(i != a && i != b)
                Destroy(anims[i].gameObject);
        plScale = player.transform.localScale;
        rival.transform.localScale = plScale - new Vector3(2*plScale.x, 0, 0);
        rival.transform.localPosition -= new Vector3(rival.transform.localPosition.x * 2, 0, 0);
    }

    public void SwitchColors(){
        tempo = 1;
        rival.transform.localScale = plScale;
        player.transform.localScale = plScale - new Vector3(2*plScale.x, 0, 0);
        rival.transform.localPosition -= new Vector3(rival.transform.localPosition.x * 2, 0, 0);
        player.transform.localPosition -= new Vector3(player.transform.localPosition.x * 2, 0, 0);
        enemyObject.transform.localPosition -= new Vector3(enemyObject.transform.localPosition.x * 2, 0, 0);
        GameObject aux = player;
        player = rival;
        rival = aux;
    }

    public void PlayAnim(){
        enemyObject.SetActive(true);
        StartCoroutine("StopAnim");
    }

    public IEnumerator StopAnim(){
        yield return new WaitForSeconds(0.8f);
        if(ballTaken){
            ball.SetActive(false);
            enemyBall.SetActive(true);
        }
        else ball.GetComponent<Animator>().SetTrigger("Carrinho");
        yield return new WaitForSeconds(0.67f);
        enemyObject.SetActive(false);
        if(ballTaken){
            ball.SetActive(true);
            enemyBall.SetActive(false);
            ballTaken = false;
        }
    }

    public void MovePlayer(int noteHit){
        lastHitNote = noteHit;
        if(noteHit > 1 && (gm.scene == "PR" || gm.scene == "RR"))
            return;

        switch(gm.scene){
            case "PR":
                mod = new Vector3(1, 0, 0);
                transform.position = rc.transform.GetChild(0).GetChild(2).position - new Vector3(.7f, 0, 0);
                anchor = rc.transform.GetChild(0).GetChild(2).position - new Vector3(.3f, -1.5f, 0);
                ball.SetActive(true);
                player.SetActive(true);
                rival.SetActive(false);
                if(scaled == -1){
                    enemyObject.transform.localScale = new Vector3(1.44043f, 1.44043f, 1.44043f);
                    ball.transform.localScale = new Vector3(0.21f, 0.21f, 0.21f);
                    ball.transform.localPosition = new Vector3(0.75f, -3.44f, 0f);
                    scaled = 1;
                }
                break;
            case "RR":
                mod = new Vector3(-1, 0, 0);
                transform.position = rc.transform.GetChild(0).GetChild(2).position + new Vector3(.7f, 0, 0);
                anchor = rc.transform.GetChild(0).GetChild(2).position + new Vector3(.3f, 1.5f, 0);
                // anchor = rc.transform.GetChild(noteHit).GetChild(2).position - new Vector3(.7f, 0, 0);
                ball.SetActive(true);
                rival.SetActive(true);
                player.SetActive(false);
                if(scaled == 1){
                    enemyObject.transform.localScale = new Vector3(-1.44043f, 1.44043f, 1.44043f);
                    ball.transform.localScale = new Vector3(-0.21f, 0.21f, 0.21f);
                    ball.transform.localPosition = new Vector3(-0.75f, -3.44f, 0f);
                    scaled = -1;
                }
                break;
            case "TACKLE":
                mod = new Vector3(((noteHit % 2 == 0) ? 1 : -1 ), 0, 0);
                anchor = rc.transform.GetChild(noteHit).GetChild(2).position;
                player.SetActive(false);
                rival.SetActive(false);
                ball.SetActive(false);
                break;
            case "RGK":
                mod = new Vector3(0, 1, 0);
                anchor = rc.transform.GetChild(noteHit).GetChild(2).position + new Vector3(0, 0.5f, 0);
                player.SetActive(false);
                rival.SetActive(false);
                ball.SetActive(false);
                break;
            case "PGK":
                mod = new Vector3(0, -1, 0);
                anchor = rc.transform.GetChild(noteHit).GetChild(2).position - new Vector3(0, 0.5f, 0);
                rival.SetActive(false);
                player.SetActive(false);
                ball.SetActive(false);
                break;
        }
    }

    public void HitNoteScore(string name){
        StartCoroutine(HitText(name));
    }

    public IEnumerator HitText(string name){
        yield return new WaitForSeconds(0.05f);
        switch(name){
            case "Ok":
                Instantiate(scoreObjs[0], anchor + mod, Quaternion.identity);
                break;
            case "Great":
                Instantiate(scoreObjs[1], anchor + mod, Quaternion.identity);
                break;
            case "Perfect":
                Instantiate(scoreObjs[2], anchor + mod, Quaternion.identity);
                break;
        }
        if(gm.scene == "RR" && tempo == 0)
            rival.GetComponent<AnimController>().RivalGettingHit(lastHitNote);
        else if(gm.scene == "PR" && tempo == 1)
            player.GetComponent<AnimController>().RivalGettingHit(lastHitNote);
    }
}
