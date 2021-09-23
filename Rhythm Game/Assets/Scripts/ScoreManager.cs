using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RhythmGameStarter;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private Text timer;
    private MapManager mm;
    public int currentTime = 0;
    public float startTime = 0;
    public float score = 0, scoreGoal; // ScoreGoal é o valor máximo da barra de progresso
    public bool destroyNotes = false, getScore = true, songFinished = false, segundoTempo = false;
    public int[] gols = new int[2];
    // private List<GameObject> notesToDestroy = new List<GameObject>();

    void Start() {
        mm = this.GetComponent<MapManager>();
    }

    void Update() {
        currentTime = Mathf.FloorToInt((Time.fixedTime - startTime) / 2.27f);
        timer.text = $"{currentTime:00}:00";
        if(songFinished && !segundoTempo){
            segundoTempo = true;
            currentTime = 0;
            songFinished = false;
            mm.SegundoTempo();
            startTime = Time.fixedTime + 1;
        }
    }

    public void SongFinished(){
        if(songFinished){
            mm.animPanel.SetActive(true);
            mm.animPanel.transform.GetChild(0).GetComponent<Text>().text = "Partida acabou";
            mm.StopCoroutine("AutoAction");
        }
        songFinished = true;
    }

    // Preencher barra de progresso
    public void AddScore(string acc){
        if(!getScore)
            return;
        score += Mathf.Clamp(10 * float.Parse(acc), 7, 15);
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    // Reduzir barra de progresso
    public void ReduceScore(string scoreMissed){
        if(!getScore)
            return;
        score -= 5;
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    public void UseScore(float valueUsed){
        score -= valueUsed;
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    // Resetar os pontos da barra de progresso
    public void ResetScore(){
        score = 0;
        progressBar.fillAmount = score / scoreGoal;
    }

    // Impede as notas de aparecerem muito rápido ao trocar de cena
    public void DestroyNotes(Note note){
        if(destroyNotes){
            // notesToDestroy.Add(note.gameObject);
            // note.gameObject.tag = "Untagged";
            note.gameObject.SetActive(false);
        }
    }

    // public IEnumerator ClearNotes(){
    //     yield return new WaitForSeconds(1);
    //     for(int i = 0; i < notesToDestroy.Count; i++){
    //         Destroy(notesToDestroy[i]);
    //     }
    //     notesToDestroy = new List<GameObject>();
    // }
}
