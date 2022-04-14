using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePositionsChange : MonoBehaviour
{
    void Start(){
        int rnd = Random.Range(0, 100);
        print(rnd);
        if(rnd <= 50){
            for(int i = 0; i < 4; i++){
                Transform child = transform.GetChild(i);
                child.Rotate(0, 0, 90);
                child.position = new Vector3(child.position.x, 1.05f + (1.05f * i), 0);
            }
        }
    }
}
