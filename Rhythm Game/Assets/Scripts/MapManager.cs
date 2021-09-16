using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour{
    public int ballPosition = 3; // 0, 1, 2, 3, 4, 5, 6
    [SerializeField] private GameObject ball;
    public bool withEnemy = false;
    private Vector3 centerPos;
    private float deslocamento;

    void Start(){
        centerPos = ball.GetComponent<RectTransform>().localPosition;
        deslocamento = ball.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 7;
    }

    void Update(){
        
    }

    public void MoveBall(int movement){
        float toMove = deslocamento * movement * ((withEnemy) ? -1 : 1);
        print(toMove);
        ball.GetComponent<RectTransform>().localPosition += new Vector3(toMove, 0, 0);
        if(movement == 1){
            if(Random.Range(0, 100) < 40){
                if(withEnemy){
                    withEnemy = false;
                    ball.GetComponent<Image>().color = new Color32(0, 238, 255, 255);
                }
                else{
                    withEnemy = true;
                    ball.GetComponent<Image>().color = new Color32(255, 53, 0, 255);
                }
            }
        }
    }
}
