using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{

    private Animator anim;
    private string selfTag;

    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("GetTag", 0.5f);
    }

    void Update()
    {
        Punch();
        Kick();            
    }

    private void Punch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(selfTag == "enemy")
                return;

            int randPunch = Random.Range(0, 3);

            if (randPunch == 0)
            {
                anim.SetTrigger("Soco1");
            }
            else if (randPunch == 1)
            {
                anim.SetTrigger("Soco2");
            }
            else if (randPunch == 3)
            {
                anim.SetTrigger("Soco3");
            }
        }
    }

    private void Kick()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(selfTag == "enemy")
                return;

            int randKick = Random.Range(0, 3);

            if (randKick == 0)
            {
                anim.SetTrigger("Chute1");
            }

            else if (randKick == 1)
            {
                anim.SetTrigger("Chute2");
            }
            else if (randKick == 2)
            {
                anim.SetTrigger("Chute3");
            }
        }
    }

    public void RivalGettingHit(int noteHit){
        if(noteHit == 0){
            anim.SetTrigger("DanoCima");
        }
        else if(noteHit == 1){
            anim.SetTrigger("DanoBaixo");
        }
    }

    public void GetTag(){
        selfTag = this.gameObject.tag;
    }
}
