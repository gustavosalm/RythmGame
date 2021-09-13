using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Scene scene;

    public Vector2 targetPos;
    public float Yincrement;
    public float speed;
    public float maxHeight;
    public float minHeight;
    
    
    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene();

        Movement(scene.name);
        
    }

    void Movement(string scenename)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W) && transform.position.y < maxHeight)
        {
            targetPos = new Vector2(transform.position.x, transform.position.y + Yincrement);
            //Debug.Log("UP");
        }

        if (Input.GetKeyDown(KeyCode.S) && transform.position.y > minHeight)
        {
            targetPos = new Vector2(transform.position.x, transform.position.y - Yincrement);
        }

        /*switch (scenename)
        {
            case "PR":
                if (Input.GetKeyDown(KeyCode.W) && transform.position.y < maxHeight)
                {
                    targetPos = new Vector2(transform.position.x, transform.position.y + Yincrement);
                }
                
                if (Input.GetKeyDown(KeyCode.S) && transform.position.y > minHeight)
                {
                    targetPos = new Vector2(transform.position.x, transform.position.y - Yincrement);
                }
                
                break;

             case "PGK":
                if (Input.GetKeyDown(KeyCode.W) && transform.position.y < maxHeight)
                {
                    targetPos = new Vector2(transform.position.x, transform.position.y + Yincrement);
                }

                if (Input.GetKeyDown(KeyCode.S) && transform.position.y > minHeight)
                {
                    targetPos = new Vector2(transform.position.x, transform.position.y - Yincrement);
                }

                break;
        }*/

    }
}
