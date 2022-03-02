using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelection : MonoBehaviour
{
    [SerializeField]private GameObject[] childs;

    void Start(){
        childs[Random.Range(0, 4)].SetActive(true);
    }
}
