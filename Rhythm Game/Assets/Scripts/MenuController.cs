using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public int seuTime = -1, rival = -1;
    [SerializeField] private Text title;

    public void StartQuickMatch(){
        SceneManager.LoadScene("StartUp");
    }

    public void SetTeam(int a){
        if(seuTime < 0){
            seuTime = a;
            title.text = "Selecione o time rival";
        }
        else {
            rival = a;
            StartQuickMatch();
        }
    }
}
