using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
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
}
