using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioMovement : MonoBehaviour
{
    [SerializeField]private int speed;
    [SerializeField]private List<GameObject> scenarios = new List<GameObject>();

    void Start(){
        InvokeRepeating("NextScenario", 12.5f, 12.5f);
        // if(speed < 0){
        //     scenarios[1].transform.localPosition = scenarios[0].transform.localPosition - new Vector3(1000, 0);
        //     scenarios[2].transform.localPosition = scenarios[1].transform.localPosition - new Vector3(1000, 0);
        // }
    }

    void Update(){
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void NextScenario(){
        scenarios.Add(Instantiate(scenarios[0], transform.position, Quaternion.identity));
        scenarios[3].transform.SetParent(gameObject.transform, false);
        // scenarios[3].transform.localScale = new Vector3(1, 1, 1);
        scenarios[3].transform.localPosition = scenarios[2].transform.localPosition + new Vector3(1100 * ((speed > 0) ? 1 : -1), 0);
        Destroy(scenarios[0].gameObject);
        scenarios.RemoveAt(0);
    }
}
