using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsSetSkin : MonoBehaviour
{
    [SerializeField]private SpriteRenderer[] objects;
    [SerializeField]private Sprite[] brasil, alemanha, argentina, italia;
    private int[] states = {1, 1, 1, 1, 1};
    private Sprite[][] times;
    public int seuTime, rival;
    public int ballState = 0, previousState, tempo = 1;
    [Header("comemoração")]
    [SerializeField]private SpriteRenderer[] comem;
    [SerializeField]private Sprite[] timesComem;

    void Start(){
        GameObject menuObj = GameObject.FindWithTag("MenuController");
        if(menuObj){
            MenuController mc = menuObj.GetComponent<MenuController>();
            seuTime = mc.seuTime;
            rival = mc.rival;
            Destroy(mc.gameObject);
        }
        times = new Sprite[4][]{brasil, alemanha, argentina, italia};
        objects[2].sprite = times[rival][2];
        // objects[0].sprite = times[seuTime][0];
    }

    public void SetSkin(int obj, int team){
        previousState = ballState;
        if(obj == 2) {
            ballState = team;
            return;
        }
        else if(obj > 4) {
            ballState = team;
            return;
        }
        else if(ballState == 2)
            ballState = team;
        team = (team == 2) ? 1 : ((team == 3) ? -1 : team);
        objects[obj].sprite = times[((ballState == -1) ? rival : seuTime)][obj];

        if((ballState * tempo) != states[obj]){
            objects[obj].gameObject.transform.parent.localScale = Vector3.Scale(objects[obj].gameObject.transform.parent.localScale, new Vector3(-1, 1, 1));
            objects[obj].gameObject.transform.parent.position = Vector3.Scale(objects[obj].gameObject.transform.parent.position, new Vector3(-1, 1, 1));
            states[obj] *= -1;
        }

        ballState = team;
    }

    public void AnimPosGol(){
        int spriteInd = ((previousState == 1) ? seuTime : rival) * 2;
        comem[0].sprite = timesComem[spriteInd];
        comem[1].sprite = timesComem[spriteInd+1];
    }
}
