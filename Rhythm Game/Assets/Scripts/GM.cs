using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using RhythmGameStarter;


public class GM : MonoBehaviour
{
    public GameObject[] Tracks;
    string scene;
    private ScoreManager sm;
    private MapManager mm;
    public string changeScene = "";

    // Start is called before the first frame update
    void Start()
    {
        sm = this.GetComponent<ScoreManager>();
        mm = this.GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene().name;
        
        // A cena certa é chamada no BallExit() no MapManager
        // if (scene == "Startup")
        // {
        //     SceneManager.LoadScene("PR");
        // }
        
        // rodar só uma vez
        ActiveTracks(scene);

        if (Input.GetKeyDown(KeyCode.O) && /* sm.score == sm.scoreGoal && */ mm.ballState == 1){
            // chute
            sm.ResetScore();
            sm.getScore = false;
            mm.MoveBall(2);
        }
        else if (Input.GetKeyDown(KeyCode.P) && /* sm.score == sm.scoreGoal && */ mm.ballState == 1){
            // passe
            sm.ResetScore();
            sm.getScore = false;
            mm.MoveBall(1);
        }
        // else if(mm.ballState == 0 && sm.score == sm.scoreGoal){
        //     // quando ta em disputa de bola
        //     sm.ResetScore();
        //     mm.MoveBall(1);
        // }
        // else if(mm.ballState == -1 && sm.score == sm.scoreGoal){
        //     // quanto ta com o rival
        //     sm.ResetScore();
        //     int prob = Random.Range(0, 100);
        //     mm.MoveBall((prob < 50) ? 1 : 2);
        // }

        // Troca a cena no início do frame
        if(changeScene != ""){
            SceneManager.LoadScene(changeScene);
            changeScene = "";
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("PR");
        }
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     SceneManager.LoadScene("PGK");
        // }
        // else if (Input.GetKeyDown(KeyCode.Y))
        // {
        //     SceneManager.LoadScene("RR");
        // }
        // else if (Input.GetKeyDown(KeyCode.U))
        // {
        //     SceneManager.LoadScene("RGK");
        // }
        // else if (Input.GetKeyDown(KeyCode.I))
        // {
        //     SceneManager.LoadScene("TACKLE");
        // }
        // else if (Input.GetKeyDown(KeyCode.G))
        // {
        //     SceneManager.LoadScene("PR");
        // }
    }

    public void ChangeScene(string sceneName){
        SceneManager.LoadScene(sceneName);
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
