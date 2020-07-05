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
    public generalScript Engine;
    debug_ui debugScript;
    public GameObject ball;
    GameObject[] bases;
    public touch_input touch;
    public string updateLineGate;
    public float animSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("ball");
        ballScript = ball.GetComponent<BallControl>();
        bases = GameObject.FindGameObjectsWithTag("base");
        touch = GetComponent<touch_input>();
        Engine = GetComponent<generalScript>();
        debugScript = GetComponent<debug_ui>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Engine.fail&&!Engine.win){
            if(Engine.way.Count==2 && PathCoordinates.Count!=2){
                CreateLine();
            }
            updateLineGate = ((Engine.way.Count>2).ToString()+ballScript.docked.ToString());
            if(Engine.way.Count>2   &&   Engine.way.Count>LineRender.positionCount  &&  ballScript.docked){
                UpdateLine(bases[Engine.way[Engine.way.Count-1]].transform.position);
            }
        }
    }
    void CreateLine(){
        // Debug.Log("**Line Created**");
        currentLine =  Instantiate(PathLine, Vector3.zero, Quaternion.identity);
        LineRender = currentLine.GetComponent<LineRenderer>();
        LineRender.positionCount++;
        PathCoordinates.Add(bases[Engine.way[0]].transform.position);
        LineRender.SetPosition(LineRender.positionCount -1, PathCoordinates[PathCoordinates.Count-1]);
        LineRender.positionCount++;
        PathCoordinates.Add(bases[Engine.way[1]].transform.position);
        LineRender.SetPosition(LineRender.positionCount -1, PathCoordinates[PathCoordinates.Count-1]);
    }

    void UpdateLine(Vector2 newPath){
        // Debug.Log("Line Updated : " + Engine.way[Engine.way.Count-1]);
        PathCoordinates.Add(newPath);
        LineRender.positionCount++;
        LineRender.SetPosition(LineRender.positionCount -1, newPath);
    }

    public void getBack(){

        List<int[]> wayCpls = Engine.wayCouples;
        Vector2 wayBack = LineRender.GetPosition(wayCpls.Count-1);

        Vector2 baseF = bases[  Engine.way[Engine.way.Count-2 ] ].transform.position; //Base FROM
        Vector2 baseT= bases[   Engine.way[Engine.way.Count-1] ].transform.position; //Base TO
        
        Debug.Log("X/Y"+(baseF.x-baseT.x)+"--"+(baseF.y-baseT.y));
        wayBack.x = (LineRender.GetPosition(LineRender.positionCount-1).x) +   (baseF.x - baseT.x)*animSpeed;
        wayBack.y = (LineRender.GetPosition(LineRender.positionCount-1).y) +   (baseF.y - baseT.y)*animSpeed;
        LineRender.SetPosition(LineRender.positionCount-1,wayBack);

        float deltaLineX = Mathf.Abs(LineRender.GetPosition(Engine.way.Count-2).x-LineRender.GetPosition(Engine.way.Count-1).x);
        float deltaLineY = Mathf.Abs(LineRender.GetPosition(Engine.way.Count-2).y-LineRender.GetPosition(Engine.way.Count-1).y);

        if ( deltaLineX<15 && deltaLineY<15)
        {
            removeLastPath();
        }

        if(LineRender.positionCount==1){
            Destroy(GameObject.FindGameObjectWithTag("Lines"));
            Engine.way.Clear();
            Engine.wayCouples.Clear();
            PathCoordinates.Clear();
            ballScript.ivmeX=0;
            ballScript.ivmeY=0;
            Engine.way.Add(ballScript.nearBase);
            Engine.fail = false;
        }
    }
    public void removeLastPath(){
        LineRender.positionCount--;
        Engine.wayCouples.RemoveAt(Engine.wayCouples.Count-1);
        Engine.way.RemoveAt(Engine.way.Count-1);
    }
}