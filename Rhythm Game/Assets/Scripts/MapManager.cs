using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int ballPosition = 3; // 0, 1, 2, 3, 4, 5, 6
    [SerializeField] private GameObject ball;
    private Vector3 centerPos;
    private float deslocamento;

    void Start()
    {
        centerPos = ball.GetComponent<RectTransform>().localPosition;
        deslocamento = ball.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 7;
        // ball.GetComponent<RectTransform>().localPosition -= new Vector3(deslocamento, 0, 0);
    }

    void Update()
    {
        
    }

    public void MoveBall(int movement){
        ball.GetComponent<RectTransform>().localPosition += new Vector3(deslocamento * movement, 0, 0);
    }
}
