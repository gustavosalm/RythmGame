using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RhythmGameStarter;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    public float score = 0, scoreGoal;
    public bool destroyNotes = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(string acc){
        score += 10 * float.Parse(acc);
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    public void ReduceScore(string scoreMissed){
        score -= 5;
        score = Mathf.Clamp(score, 0, scoreGoal);
        progressBar.fillAmount = score / scoreGoal;
    }

    public void ResetScore(){
        score = 0;
        progressBar.fillAmount = score / scoreGoal;
    }

    public void DestroyNotes(Note note){
        if(destroyNotes){
            note.gameObject.SetActive(false);
        }
    }
}
