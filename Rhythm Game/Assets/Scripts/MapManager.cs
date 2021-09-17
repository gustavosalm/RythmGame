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
    private string[] anims = {"animação passe", "animação chute", "disputa de bola", "meu time recebe", "rival recebe", "goleiro chuta"};
    private string[] sceneNames = {"RR", "TACKLE", "PR", "RGK", "PGK"};

    void Start(){
        centerPos = ball.GetComponent<RectTransform>().localPosition;
        deslocamento = ball.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 7;

        BallExit();
    }

    void Update(){
        
    }

    public void MoveBall(int movement){
        if(ballState != 0 && movement == 2 && ((ballState == 1) ? ballPosition >= 4 : ballPosition <= 2)){
            ChuteAoGol();
            return;
        }

        ball.GetComponent<RectTransform>().localPosition += new Vector3(deslocamento * movement * ballState, 0, 0);
        ballPosition += movement * ballState;

        List<string> animsList = new List<string>();

        int prob = Random.Range(0, 100);
        if(ballState == 0){
            // Disputa de bola não tem probabilidade?
            ballState = (prob < 60) ? 1 : -1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[(ballState == 1) ? 3 : 4]);
        }
        else if((movement == 1) ? prob < 30 : prob < 50){
            ballState *= (prob < 30) ? -1 : 1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[(movement == 1) ? 0 : 1]);
            animsList.Add(anims[(ballState == 1) ? 3 : 4]);
        }
        else if((movement == 1) ? prob < 60 : prob < 70){
            ballState = 0;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[(movement == 1) ? 0 : 1]);
            animsList.Add(anims[2]);
        }
        else {
            animsList.Add(anims[(movement == 1) ? 0 : 1]);
            animsList.Add(anims[(ballState == 1) ? 3 : 4]);
        }

        this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        this.GetComponent<ScoreManager>().ResetScore();
        StartCoroutine(PlayAnim(animsList));
    }

    public IEnumerator PlayAnim(List<string> anim){
        string message = string.Join("\n", anim);
        animPanel.SetActive(true);
        animPanel.transform.GetChild(0).GetComponent<Text>().text = message;
        yield return new WaitForSeconds(1);
        animPanel.SetActive(false);
    }

    public void ChuteAoGol(){
        List<string> animsList = new List<string>();
        animsList.Add(anims[1]);

        int prob = Random.Range(0, 100);
        if(prob < 40){
            // pra fora
            ball.GetComponent<RectTransform>().localPosition = centerPos + new Vector3(deslocamento * 1 * ballState, 0, 0);
            ballState *= (prob < 60) ? -1 : 1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[5]);
            animsList.Add(anims[(ballState == 1) ? 3 : 4]);
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        }
        else /*if(prob < 60)*/{
            ballState = 0;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[2]);
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        }
        // else{
        //     // tentando gol
        // }
        // this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        this.GetComponent<ScoreManager>().ResetScore();
        StartCoroutine(PlayAnim(animsList));
    }

    void BallExit(){
        int prob = Random.Range(0, 100);
        ballState *= (prob < 50) ? -1 : 1;
        ball.GetComponent<Image>().color = states[ballState + 1];
        this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
    }
}
