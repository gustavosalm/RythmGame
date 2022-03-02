using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RhythmGameStarter;

public class MapManager : MonoBehaviour{
    [SerializeField] public GameObject ball, animPanel, canva; // UI da bola no campo e alerta de animação
    [SerializeField] private SongManager songManager;
    [SerializeField] private float enemyTakeTime, enemyTakeAmount;
    public float ballSpeed;
    private ScoreManager sm;
    [HideInInspector] public int ballPosition = 3; // 0, 1, 2, 3, 4, 5, 6
    [HideInInspector] public int ballState = 1; // -1 rival | 0 disputa de bola | 1 seu time
    private Vector3 centerPos; // meio de campo
    private float deslocamento, gol; // o tanto que a bola anda por passe

    // cores pra eu saber o estado que a bola tá (apenas pra testes)
    private Color32[] states = {new Color32(255, 53, 0, 255), new Color32(213, 255, 0, 255), new Color32(0, 238, 255, 255)};
    // texto das animações que vão ser chamadas (vai ser substituído pelo nome da animação em si quando tiver)
    private string[] anims = {"animação passe", "animação chute", "disputa de bola", "meu time recebe", "rival recebe", "goleiro chuta", "animação gol", "animação goleiro defende", "saída de bola", "Rival rouba a bola", "Player perdeu a bola", "Segundo Tempo"};
    [SerializeField]
    private GameObject[] animObjs;
    // nome das cenas para ficar mais fácil de chamar elas
    private string[] sceneNames = {"RR", "TACKLE", "PR", "RGK", "PGK"};

    [Header("Probabilidades PR Passe")]
    public int rivalRecepePasse0;
    public int rivalRecepePasse100;
    public int disputaPasse0;
    public int disputaPasse100;

    [Header("Probabilidades PR Chute Longe")]
    public int rivalRecebeChute0;
    public int rivalRecebeChute100;
    public int disputaChute0;
    public int disputaChute100;

    [Header("Probabilidades PR Chute Perto")]
    public int PRChuteAoGol0;
    public int PRChuteAoGol100;

    [Header("Probabilidades RR Passe")]
    public int playerRecepePasse0;
    public int playerRecepePasse100;
    public int RRDisputaPasse0;
    public int RRDisputaPasse100;

    [Header("Probabilidades RR Chute Longe")]
    public int playerRecebeChute0;
    public int playerRecebeChute100;
    public int RRDisputaChute0;
    public int RRDisputaChute100;

    [Header("Probabilidades RR Chute Perto")]
    public int RRChuteAoGol0;
    public int RRChuteAoGol100;

    [Header("Probabilidades Tackle")]
    public int playerRecebeTackle0;
    public int playerRecebeTackle100;

    [Header("Probabilidades PGK")]
    public int rivalGol0;
    public int rivalGol100;
    public int rivalRecebeDefesa0;
    public int rivalRecebeDefesa100;

    [Header("Probabilidades RGK")]
    public int playerGol0;
    public int playerGol100;
    public int playerRecebeDefesa0;
    public int playerRecebeDefesa100;

    void Start(){
        animPanel.SetActive(false);

        centerPos = ball.GetComponent<RectTransform>().localPosition;
        deslocamento = ball.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 7;
        gol = ball.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2;
        sm = this.GetComponent<ScoreManager>();

        BallExit();
    }

    void Update(){
        bool noGol = (ballState == 1) ? ballPosition > 5 : ballPosition < 1;
        if(ballState <= 1 && !noGol)
            ball.transform.Translate(Vector3.right * ballSpeed * ballState * Time.deltaTime);
    }

    public void MoveBall(int movement){
        StopCoroutine("EnemyTake");
        if(ballState <= 1)
            ballPosition += ballState;

        // Chute direcionado ao gol
        if(ballState != 0 && movement == 2 && ((ballState == 1) ? ballPosition > 4 : ballPosition < 2)){
            ChuteAoGol();
            return;
        }

        // Move a bola na direção do gol
        int limitador = ((ballState == 1) ? ballPosition >= 5 : ballPosition <= 1) ? 0 : 1;
        ball.GetComponent<RectTransform>().localPosition += new Vector3(deslocamento * movement *  limitador * ballState, 0, 0);
        ballPosition += movement * ballState * limitador;

        // Lista de animações que vão rodar
        List<int> animsList = new List<int>();

        // Probabilidade e modificador baseado em quão cheia está a barra
        int prob = Random.Range(0, 100);
        float barMod = sm.score / sm.scoreGoal;
        print(barMod);
        if(ballState == 0){
            // Acabar a disputa de bola
            int tackleMod = (int)(playerRecebeTackle0 + (barMod * (playerRecebeTackle100 - playerRecebeTackle0)));
            ballState = (prob < tackleMod) ? 1 : -1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add((ballState == 1) ? 3 : 4);
            sm.ResetScore();
        }
        else{
            int probDiff;
            int secProbDiff;
            int initialState = ballState;
            if(movement == 1){
                probDiff = (int)((ballState == 1) ? rivalRecepePasse0 - (barMod * (rivalRecepePasse0 - rivalRecepePasse100)) : playerRecepePasse0 + (barMod * (playerRecepePasse100 - playerRecepePasse0)));
                secProbDiff = (int)((ballState == 1) ? disputaPasse0 - (barMod * (disputaPasse0 - disputaPasse100)) : RRDisputaPasse100 - (barMod * (RRDisputaPasse100 - RRDisputaPasse0)));
            }
            else{
                probDiff = (int)((ballState == 1) ? rivalRecebeChute0 - (barMod * (rivalRecebeChute0 - rivalRecebeChute100)) : playerRecebeChute0 + (barMod * (playerRecebeChute100 - playerRecebeChute0)));
                secProbDiff = (int)((ballState == 1) ? disputaChute0 - (barMod * (disputaChute0 - disputaChute100)) : RRDisputaChute100 + (barMod * (RRDisputaChute100 - RRDisputaChute0)));
            }
            if(prob < probDiff){
                // Chance da bola trocar de time
                ballState *= -1;
                ball.GetComponent<Image>().color = states[ballState + 1];
                animsList.Add((movement == 1) ? 0 : 1);
                animsList.Add((ballState == 1) ? 3 : 4);
            }
            else if(prob < secProbDiff + probDiff){
                // Chance de virar uma disputa de bola
                ballState = 0;
                ball.GetComponent<Image>().color = states[ballState + 1];
                animsList.Add((movement == 1) ? 0 : 1);
                animsList.Add(2);
            }
            else {
                // A bola continua no mesmo time
                animsList.Add((movement == 1) ? 0 : 1);
                animsList.Add((ballState == 1) ? 3 : 4);
            }
            if(initialState == -1)
                sm.ResetScore();
        }

        // Se a bola não estiver com o jogador, o código sorteia um tempo até acontecer algo
        if(ballState != 1)
            StartCoroutine("AutoAction");
        else if(ballState == 1)
            StartCoroutine("EnemyTake");

        // Começar próxima cena e roda as animações
        this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        StartCoroutine(PlayAnim(animsList));
    }

    public IEnumerator PlayAnim(List<int> anim){
        animPanel.SetActive(true);
        canva.SetActive(false);
        for(int i = 0; i < anim.Count; i++){
            if(anim[i] >= 9) break;
            string message = anims[anim[i]]; // string.Join("\n", anim);
            GameObject currentAnim = animObjs[anim[i]];
            currentAnim.SetActive(true);
            animPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = message;
            yield return new WaitForSeconds(0.9f);
            currentAnim.SetActive(false);
        }
        sm.getScore = true;
        animPanel.SetActive(false);
        canva.SetActive(true);
    }

    public IEnumerator AutoAction(){
        sm.ResetScore();
        int interval = Random.Range(8, 12);
        yield return new WaitForSeconds(interval+2);
        if(ballState == 0){
            MoveBall(1);
        }
        else if(ballState == -1){
            int prob = Random.Range(0, 100);
            MoveBall((prob < 50 && ballPosition > 1) ? 1 : 2);
        }
        else{
            List<int> animsList = new List<int>();
            int prob = Random.Range(0, 100);
            float barMod = sm.score / sm.scoreGoal;
            int probDiff = (int)((ballState == 2) ? playerGol0 + (barMod * (playerGol100 - playerGol0)) : rivalGol0 - (barMod * (rivalGol0 - rivalGol100)));
            if(prob < probDiff){
                // gol
                animsList.Add(6);
                animsList.Add(8);
                sm.gols[(ballState == 2) ? 0 : 1] += 1;
                sm.AlterarPlacar();
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
                int probMod = (int)((ballState == 1) ? playerRecebeDefesa0 + (barMod * (playerRecebeDefesa100 - playerRecebeDefesa0)) : rivalRecebeDefesa0 - (barMod * (rivalRecebeDefesa0 - rivalRecebeDefesa100)));
                ballState *= (prob < probDiff + probMod) ? 1 : -1;
                ball.GetComponent<Image>().color = states[ballState + 1];
                animsList.Add(7);
                animsList.Add(5);
                animsList.Add((ballState == 1) ? 3 : 4);
                this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
            }
            sm.ResetScore();
            if(ballState != 1)
                StartCoroutine("AutoAction");
            else if(ballState == 1)
                StartCoroutine("EnemyTake");
            StartCoroutine(PlayAnim(animsList));
        }
    }

    // Rival tenta pegar a bola se o jogador demora pra agir
    public IEnumerator EnemyTake(){
        yield return new WaitForSeconds(enemyTakeTime - .5f);
        gameObject.GetComponent<GM>().fnh.PlayAnim();
        yield return new WaitForSeconds(.5f);
        if(ballState != 1)
            yield break;
        sm.UseScore((enemyTakeAmount / 100) * sm.scoreGoal);
        if(sm.score <= (0.2f * sm.scoreGoal)){
            ballState = -1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            sm.ResetScore();
            StartCoroutine("AutoAction");
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
            StartCoroutine(PlayAnim(new List<int>() {9}));
            yield break;
        }
        StartCoroutine("EnemyTake");
    }

    public void EnemyInstaTake(){
        if(ballState != 1)
            return;

        ballState = -1;
        ball.GetComponent<Image>().color = states[ballState + 1];
        StartCoroutine("AutoAction");
        this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        StartCoroutine(PlayAnim(new List<int>() {10}));
    }
    // Chute perto do gol
    public void ChuteAoGol(){
        List<int> animsList = new List<int>();
        animsList.Add(1);

        int prob = Random.Range(0, 100);
        float barMod = sm.score / sm.scoreGoal;
        int probDiff = (int)((ballState == 1) ? PRChuteAoGol0 + (barMod * (PRChuteAoGol100 - PRChuteAoGol0)) : RRChuteAoGol0 - (barMod * (PRChuteAoGol0 - PRChuteAoGol100)));
        int secProbDiff = (int)((ballState == 1) ? disputaChute0 - (barMod * (disputaChute0 - disputaChute100)) : RRDisputaChute100 + (barMod * (RRDisputaChute100 - RRDisputaChute0)));
        if(prob < probDiff){
            // Chute ao gol
            ball.GetComponent<RectTransform>().localPosition = centerPos + new Vector3(deslocamento * 3 * ballState, 0, 0);
            ballPosition = 3 + (3 * ballState);
            ballState = (ballState == 1) ? 2 : 3;
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        }
        else if(prob < probDiff + secProbDiff){
            // Disputa de bola
            ballState = 0;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(2);
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        }
        else{
            // Chute pra fora
            ball.GetComponent<RectTransform>().localPosition = centerPos + new Vector3(deslocamento * 1 * ballState, 0, 0);
            ballPosition = 3 + ballState;
            prob = Random.Range(0, 100);
            ballState *= (prob < 60) ? -1 : 1;
            ball.GetComponent<Image>().color = states[ballState + 1];
            animsList.Add(5);
            animsList.Add((ballState == 1) ? 3 : 4);
            this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        }

        if(ballState != 1)
            StartCoroutine("AutoAction");
        else if(ballState == 1)
            StartCoroutine("EnemyTake");

        sm.ResetScore();
        StartCoroutine(PlayAnim(animsList));
    }

    public void SegundoTempo(){
        StopCoroutine("PlayAnim");
        StopCoroutine("AutoAction");
        StopCoroutine("EnemyTake");
        sm.destroyNotes = false;
        sm.ResetScore();
        gameObject.GetComponent<GM>().fnh.SwitchColors();
        StartCoroutine(PlayAnim(new List<int>() {11}));
        ball.GetComponent<RectTransform>().localPosition = centerPos;
        ballPosition = 3;
        ballState = 1;
        songManager.PlaySong();
        deslocamento *= -1;
        ballSpeed *= -1;
        sceneNames[0] = "PR";
        sceneNames[2] = "RR";
        BallExit();
    }

    // Quem começa com a bola 50/50
    void BallExit(){
        int prob = Random.Range(0, 100);
        ballState *= (prob < 50) ? -1 : 1;
        ball.GetComponent<Image>().color = states[ballState + 1];
        this.GetComponent<GM>().changeScene = sceneNames[ballState + 1];
        if(ballState != 1)
            StartCoroutine("AutoAction");
        else if(ballState == 1){
            StartCoroutine("EnemyTake");
        }
    }
}
