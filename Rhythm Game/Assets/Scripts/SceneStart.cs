using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmGameStarter;

public class SceneStart : MonoBehaviour
{
    public GameObject dummyPos;
    public float poolLookAheadTime; //tempo entre a nota aparecer e ela passar pela line area
    public SongItem songItem; //songitem espec�fico da cena atual
    public Transform[] trackPos, newTrackPos; //array com as posi��es do rhythmcore e do dummy position

    GameObject rhythmCore, gameManager; //refer�ncias aos respectivos gameobjects
    TrackManager trackManager; 
    SongManager songManager;

    // Start is called before the first frame update
    void Start()
    {
        //criando as refer�ncias para utilizar no restante do scrpit
        rhythmCore = GameObject.Find("RhythmCore 1");
        gameManager = GameObject.Find("Game Manager");
        trackManager = rhythmCore.GetComponent<TrackManager>();
        songManager = rhythmCore.GetComponent<SongManager>();

        //setup do tempo da nota aparecer e do songitem na cena atual
        trackManager.poolLookAheadTime = poolLookAheadTime;

        //Atualizo o songitem usado na cena
        UpdateSong();
        TrackPosition();
    }

    void TrackPosition()
    {
        dummyPos.SetActive(true);

        trackPos = new Transform[5];
        newTrackPos = new Transform[5];

        for (int i = 0; i < 5; i++)
        {
            trackPos[i] = rhythmCore.transform.GetChild(i);
            newTrackPos[i] = dummyPos.transform.GetChild(i);
        }

        for (int i = 0; i < trackPos.Length; i++)
        {
            trackPos[i].transform.position = newTrackPos[i].position;
            trackPos[i].transform.rotation = newTrackPos[i].localRotation;

            for (int j = 0; j < trackPos[i].childCount; j++)
            {
                trackPos[i].transform.GetChild(j).transform.position = newTrackPos[i].transform.GetChild(j).transform.position;
                trackPos[i].transform.GetChild(j).transform.rotation = newTrackPos[i].transform.GetChild(j).transform.rotation;
            }
        }

        dummyPos.SetActive(false);
    }

    void UpdateSong()
    {
        songManager.defaultSong = songItem;
        songManager.currnetNotes = songItem.GetNotes();
        gameManager.GetComponent<ScoreManager>().destroyNotes = true;
        StartCoroutine("ResetNoteSpawn");
        trackManager.SetUpNotePool();
        for (int i = 0; i < 4; i++)
        {
            rhythmCore.transform.GetChild(i).GetChild(3).GetComponent<NoteArea>().ResetNoteArea();
        }
    }
    public IEnumerator ResetNoteSpawn(){
        yield return new WaitForSeconds(.5f);
        gameManager.GetComponent<ScoreManager>().destroyNotes = false;
        // StartCoroutine(gameManager.GetComponent<ScoreManager>().ClearNotes());
    }
}
