using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RhythmGameStarter;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    public float score = 0, scoreGoal; // ScoreGoal é o valor máximo da barra de progresso
    public bool destroyNotes = false, getScore = true;
    public int[] gols = new int[2];
    // private List<GameObject> notesToDestroy = new List<GameObject>();

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
        score -= 5;
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    // Resetar os pontos da barra de progresso
    public void ResetScore(){
        score = 0;
        progressBar.fillAmount = score / scoreGoal;
    }

    // Reseta os pontos no final do frame pra evitar um certo erro
    public IEnumerator ResetScoreValue(){
        yield return new WaitForEndOfFrame();
        score = 0;
    }

    // Impede as notas de aparecerem muito rápido ao trocar de cena
    public void DestroyNotes(Note note){
        if(destroyNotes){
            // notesToDestroy.Add(note.gameObject);
            note.gameObject.tag = "Untagged";
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
