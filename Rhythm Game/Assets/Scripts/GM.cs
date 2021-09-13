using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using RhythmGameStarter;


public class GM : MonoBehaviour
{
    public GameObject[] Tracks;
    string scene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene().name;
        
        if (scene == "Startup")
        {
            SceneManager.LoadScene("PR");
        }
        
        ActiveTracks(scene);

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("PGK");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene("RR");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("RGK");
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("TACKLE");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("PR");
        }
    }

    void ActiveTracks(string scene)
    {
        switch (scene)
        {
            case "PR":
                Tracks[0].SetActive(true);
                Tracks[1].SetActive(true);
                Tracks[2].SetActive(false);
                Tracks[3].SetActive(false);
                break;

            case "PGK":
                Tracks[0].SetActive(true);
                Tracks[1].SetActive(true);
                Tracks[2].SetActive(true);
                Tracks[3].SetActive(true);
                break;

            case "RR":
                Tracks[0].SetActive(true);
                Tracks[1].SetActive(true);
                Tracks[2].SetActive(false);
                Tracks[3].SetActive(false);
                break;

            case "RGK":
                Tracks[0].SetActive(true);
                Tracks[1].SetActive(true);
                Tracks[2].SetActive(true);
                Tracks[3].SetActive(true);
                break;

            case "TACKLE":
                Tracks[0].SetActive(true);
                Tracks[1].SetActive(true);
                Tracks[2].SetActive(true);
                Tracks[3].SetActive(true);
                break;
        }
    }
   
}
