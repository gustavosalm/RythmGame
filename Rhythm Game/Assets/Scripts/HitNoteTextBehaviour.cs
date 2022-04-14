using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNoteTextBehaviour : MonoBehaviour
{
    void Start(){
        Invoke("DestroySelf", 1f);
    }

    void Update(){
        transform.Translate(Vector3.up * 0.5f * Time.deltaTime);
    }

    void DestroySelf(){
        Destroy(this.gameObject);
    }
}
