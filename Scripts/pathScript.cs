using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathScript : MonoBehaviour
{
    public List<Vector2> PathCoordinates;
    public GameObject PathLine;
    public GameObject currentLine;
    public LineRenderer LineRender;
    public BallControl ballScript;
    public GameObject ball;
    bool dockCoolDown=true;
    GameObject[] bases;
    public touch_input touch;
    public string updateLineGate;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("ball");
        ballScript = ball.GetComponent<BallControl>();
        bases = GameObject.FindGameObjectsWithTag("base");
        touch = GetComponent<touch_input>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ballScript.way.Count==2&&PathCoordinates.Count!=2){
            CreateLine();
        }
        updateLineGate = ((ballScript.way.Count>2).ToString()+dockCoolDown.ToString()+ballScript.docked.ToString());
        if(ballScript.way.Count>2   &&  dockCoolDown   &&  ballScript.docked){
            UpdateLine(bases[ballScript.way[ballScript.way.Count-1]].transform.position);
            dockCoolDown=false;
        }
        if(touch.getTouchInput("pathScript")=="Swap"){dockCoolDown=true;}
    }
    void CreateLine(){
        // Debug.Log("**Line Created**");
        currentLine =  Instantiate(PathLine, Vector3.zero, Quaternion.identity);
        LineRender = currentLine.GetComponent<LineRenderer>();
        LineRender.positionCount++;
        PathCoordinates.Add(bases[ballScript.way[0]].transform.position);
        LineRender.SetPosition(LineRender.positionCount -1, PathCoordinates[PathCoordinates.Count-1]);
        LineRender.positionCount++;
        PathCoordinates.Add(bases[ballScript.way[1]].transform.position);
        LineRender.SetPosition(LineRender.positionCount -1, PathCoordinates[PathCoordinates.Count-1]);
    }

    void UpdateLine(Vector2 newPath){
        // Debug.Log("Line Updated : " + ballScript.way[ballScript.way.Count-1]);
        PathCoordinates.Add(newPath);
        LineRender.positionCount++;
        LineRender.SetPosition(LineRender.positionCount -1, newPath);
    }
}