using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour{
    public int ballPosition = 3; // 0, 1, 2, 3, 4, 5, 6
    [SerializeField] private GameObject ball, animPanel;
    public int ballState = 1;
    private Vector3 centerPos;
    private float deslocamento;

    private Color32[] states = {new Color32(255, 53, 0, 255), new Color32(213, 255, 0, 255), new Color32(0, 238, 255, 255)};
    private string[] animNames = {"animação passe\nrival recebe", "disputa de bola", "animação passe\nmeu time recebe"};
    private string[] sceneNames = {"RR", "TACKLE", "PR"};

    void Start(){
        centerPos = ball.GetComponent<RectTransform>().localPosition;
        deslocamento = ball.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 7;
    }

    void Update(){
        
    }

    public void MoveBall(int movement){
        if(ballState != 0 && movement == 2 && ((ballState == 1) ? ballPosition >= 4 : ballPosition <= 2)){
            print("a");
            ChuteAoGol();
            return;
        }
        ball.GetComponent<RectTransform>().localPosition += new Vector3(deslocamento * movement * ballState, 0, 0);
        ballPosition += movement * ballState;
        int prob = Random.Range(0, 100);
        if(ballState == 0){
            ballState = (prob < 60) ? 1 : -1;
            ball.GetComponent<Image>().color = states[ballState + 1];
        }
        else if((movement == 1) ? prob < 30 : prob < 50){
            ballState *= (prob < 30) ? -1 : 1;
            ball.GetComponent<Image>().color = states[ballState + 1];
        }
        else if((movement == 1) ? prob < 60 : prob < 70){
            ballState = 0;
            ball.GetComponent<Image>().color = new Color32(213, 255, 0, 255);
        }
        this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        StartCoroutine(PlayAnim(movement == 2));
    }

    public IEnumerator PlayAnim(bool chute){
        animPanel.SetActive(true);
        string message = animNames[ballState + 1];
        if(chute)
            message = message.Replace("passe", "chute");
        animPanel.transform.GetChild(0).GetComponent<Text>().text = message;
        yield return new WaitForSeconds(1);
        animPanel.SetActive(false);
    }

    public void ChuteAoGol(){
        print("ao gol");
    }
}
