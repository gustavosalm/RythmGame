using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour{
    private ScoreManager sm;
    public int ballPosition = 3; // 0, 1, 2, 3, 4, 5, 6
    [SerializeField] private GameObject ball, animPanel; // UI da bola no campo e alerta de animação
    public int ballState = 1; // -1 rival | 0 disputa de bola | 1 seu time
    private Vector3 centerPos; // meio de campo
    private float deslocamento; // o tanto que a bola anda por passe

    // cores pra eu saber o estado que a bola tá (apenas pra testes)
    private Color32[] states = {new Color32(255, 53, 0, 255), new Color32(213, 255, 0, 255), new Color32(0, 238, 255, 255)};
    // texto das animações que vão ser chamadas (vai ser substituído pelo nome da animação em si quando tiver)
    private string[] anims = {"animação passe", "animação chute", "disputa de bola", "meu time recebe", "rival recebe", "goleiro chuta", "animação gol", "animação goleiro defende", "saída de bola"};
    // nome das cenas para ficar mais fácil de chamar elas
    private string[] sceneNames = {"RR", "TACKLE", "PR", "RGK", "PGK"};

    void Start(){
        centerPos = ball.GetComponent<RectTransform>().localPosition;
        deslocamento = ball.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 7;
        sm = this.GetComponent<ScoreManager>();

        BallExit();
    }

    public void MoveBall(int movement){
        // Chute direcionado ao gol
        if(ballState != 0 && movement == 2 && ((ballState == 1) ? ballPosition >= 4 : ballPosition <= 2)){
            ChuteAoGol();
            return;
        }

        // Move a bola na direção do gol
        ball.GetComponent<RectTransform>().localPosition += new Vector3(deslocamento * movement * ballState, 0, 0);
        ballPosition += movement * ballState;

        // Lista de animações que vão rodar
        List<string> animsList = new List<string>();

        // Probabilidade e modificador baseado em quão cheia está a barra
        int prob = Random.Range(0, 100);
        int barMod = (int) (30 * ((ballState == 1) ? (1 - (sm.score / sm.scoreGoal)) : (sm.score / sm.scoreGoal)));
        print(barMod);
        if(ballState == 0){
            // Acabar a disputa de bola
            ballState = (prob < 50 + barMod) ? 1 : -1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[(ballState == 1) ? 3 : 4]);
        }
        else if((movement == 1) ? prob < 30 + barMod : prob < 50 + barMod){
            // Chance da bola trocar de time
            ballState *= -1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[(movement == 1) ? 0 : 1]);
            animsList.Add(anims[(ballState == 1) ? 3 : 4]);
        }
        else if((movement == 1) ? prob < 40 + barMod : prob < 50 + barMod){
            // Chance de virar uma disputa de bola
            ballState = 0;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[(movement == 1) ? 0 : 1]);
            animsList.Add(anims[2]);
        }
        else {
            // A bola continua no mesmo time
            animsList.Add(anims[(movement == 1) ? 0 : 1]);
            animsList.Add(anims[(ballState == 1) ? 3 : 4]);
        }

        // Se a bola não estiver com o jogador, o código sorteia um tempo até acontecer algo
        if(ballState != 1)
            StartCoroutine("AutoAction");

        // Começar próxima cena e roda as animações
        this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        StartCoroutine(PlayAnim(animsList));
    }

    public IEnumerator PlayAnim(List<string> anim){
        // lista na tela as animações que vão tocar (fica 1s na tela)
        string message = string.Join("\n", anim);
        animPanel.SetActive(true);
        animPanel.transform.GetChild(0).GetComponent<Text>().text = message;
        yield return new WaitForSeconds(1);
        sm.getScore = true;
        animPanel.SetActive(false);
    }

    public IEnumerator AutoAction(){
        int interval = Random.Range(8, 12);
        yield return new WaitForSeconds(interval+2);
        if(ballState == 0){
            sm.ResetScore();
            MoveBall(1);
        }
        else if(ballState == -1){
            sm.ResetScore();
            int prob = Random.Range(0, 100);
            MoveBall((prob < 50) ? 1 : 2);
        }
        else{
            List<string> animsList = new List<string>();
            int prob = Random.Range(0, 100);
            int barMod = (int) (30 * (1 - (sm.score / sm.scoreGoal)) * ((ballState == 2) ? 1 : -1));
            if(prob < 75 - barMod){
                // gol
                animsList.Add(anims[6]);
                animsList.Add(anims[8]);
                sm.gols[(ballState == 2) ? 0 : 1] += 1;
                print($"gol placar: {sm.gols[0]} x {sm.gols[1]}");
                ballState = (ballState == 2) ? -1 : 1;
                ball.GetComponent<Image>().color = states[ballState + 1];
                ball.GetComponent<RectTransform>().localPosition = centerPos;
                ballPosition = 3;
                this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
            }
            else {
                // goleiro defende
                ballState = (ballState == 2) ? 1 : -1;
                ball.GetComponent<RectTransform>().localPosition = centerPos + new Vector3(deslocamento * 1 * ballState, 0, 0);
                ballPosition = 3 + ballState;
                prob = Random.Range(0, 100);
                ballState *= (prob < 60) ? -1 : 1;
                ball.GetComponent<Image>().color = states[ballState + 1];
                animsList.Add(anims[7]);
                animsList.Add(anims[5]);
                animsList.Add(anims[(ballState == 1) ? 3 : 4]);
                this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
            }
            if(ballState != 1)
                    StartCoroutine("AutoAction");
            sm.ResetScore();
            StartCoroutine(PlayAnim(animsList));
        }
    }

    // Chute perto do gol
    public void ChuteAoGol(){
        List<string> animsList = new List<string>();
        animsList.Add(anims[1]);

        int prob = Random.Range(0, 100);
        if(prob < 40){
            // Chute pra fora
            ball.GetComponent<RectTransform>().localPosition = centerPos + new Vector3(deslocamento * 1 * ballState, 0, 0);
            ballPosition = 3 + ballState;
            prob = Random.Range(0, 100);
            ballState *= (prob < 60) ? -1 : 1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[5]);
            animsList.Add(anims[(ballState == 1) ? 3 : 4]);
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        }
        else if(prob < 60){
            // Disputa de bola
            ballState = 0;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(anims[2]);
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        }
        else{
            // Tentando gol
            ball.GetComponent<RectTransform>().localPosition = centerPos + new Vector3(deslocamento * 3 * ballState, 0, 0);
            ballPosition = 3 + (3 * ballState);
            ballState = (ballState == 1) ? 2 : 3;
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        }

        if(ballState != 1)
            StartCoroutine("AutoAction");

        sm.ResetScore();
        StartCoroutine(PlayAnim(animsList));
    }

    // Quem começa com a bola 50/50
    void BallExit(){
        int prob = Random.Range(0, 100);
        ballState *= (prob < 50) ? -1 : 1;
        ball.GetComponent<Image>().color = states[ballState + 1];
        this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        if(ballState != 1)
            StartCoroutine("AutoAction");
    }
}
