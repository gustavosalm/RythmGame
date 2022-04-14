using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoleiroBehaviour : MonoBehaviour
{
    private int pos = 0;

    void Update(){
        if(Input.GetKeyDown(KeyCode.Q))
            MoverGoleiro(0);
        else if(Input.GetKeyDown(KeyCode.W))
            MoverGoleiro(1);
        else if(Input.GetKeyDown(KeyCode.E))
            MoverGoleiro(2);
        else if(Input.GetKeyDown(KeyCode.R))
            MoverGoleiro(3);
    }

    void MoverGoleiro(int i){
        transform.localPosition += new Vector3((i - pos) * 1.5f, 0, 0);
        pos = i;
    }
}
