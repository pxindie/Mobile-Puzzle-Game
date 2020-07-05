using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class generalScript : MonoBehaviour
{
    public UnityEngine.UI.Text failedmessage;
    GameObject ball;
    GameObject[] bases;
    BallControl ballScript;
    pathScript pathS;
    public List<int> way;
    public List<int[]> wayCouples = new List<int[]>();
    public bool fail=false,win =false;

    // Start is called before the first frame update
    void Start()
    {
        bases = GameObject.FindGameObjectsWithTag("base");
        ball = GameObject.Find("ball");
        ballScript = GetComponent<BallControl>();
        pathS = GetComponent<pathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
        playAnimation();
        adjustColors();
    }

    public void failure(){
        way.RemoveAt(way.Count-1);
        wayCouples.RemoveAt(wayCouples.Count-1);
        fail=true;
        Debug.Log("Failure");
    }

    public void winnin(){

    }

    public void playAnimation(){
        if(fail){
            pathS.getBack();
        }

    }

    public void checkFailure()
    {
        if(way.Count>1){
            if(checkSameItem()) // follow the same path again
            {
                failure();
            }
        }
    }

    bool checkSameItem(){
        for (int i = 0; i < wayCouples.Count; i++)
        {
            for (int u = i+1; u < wayCouples.Count; u++)
            {
                // Debug.Log("Control List : "+i+" -- "+u+" ("+wayCouples.Count+")");
                bool fullySame = wayCouples.ElementAt(i)[0]==wayCouples.ElementAt(u)[0]&&wayCouples.ElementAt(i)[1]==wayCouples.ElementAt(u)[1];
                bool reverseSame = wayCouples.ElementAt(i)[0]==wayCouples.ElementAt(u)[1]&&wayCouples.ElementAt(i)[1]==wayCouples.ElementAt(u)[0];
                if(fullySame||reverseSame){ return true; }
            }
        }
        return false;
    }
    void adjustColors(){
        Debug.Log("AdjustColor");
        Color current = takeColor(ball.GetComponent<Renderer>().material.color);
        for (int i = 0; i < bases.Count(); i++)
        {
            Debug.Log("BaseColorTrigger");
            bases[i].GetComponent<Renderer>().material.color=current;
        }
        ball.GetComponent<Renderer>().material.color = current;
        pathS.LineRender.SetColors(current,current);
    }
    Color takeColor(Color input){
        Vector4 lastColor = input;
        float min = 0f,max = 0.9f,plus =0.1f;
        if(fail)
        {
            lastColor.x = lastColor.x<max ? lastColor.x + plus : lastColor.x;
            lastColor.y = lastColor.y>min ? lastColor.y - plus : lastColor.y;
            lastColor.z = lastColor.z>min ? lastColor.z - plus : lastColor.z;
        }else if(win)
        {
            lastColor.x = lastColor.x>min ? lastColor.x - plus : lastColor.x;
            lastColor.y = lastColor.y<max ? lastColor.y + plus : lastColor.y;
            lastColor.z = lastColor.z<max ? lastColor.z + plus : lastColor.z;
        }else
        {
            lastColor.x = lastColor.x<max ? lastColor.x + plus : lastColor.x;
            lastColor.y = lastColor.y<max ? lastColor.y + plus : lastColor.y;
            lastColor.z = lastColor.z<max ? lastColor.z + plus : lastColor.z;
        }
        Color ret = lastColor;
        return ret;
    }
    public bool IsNegative(float x){
        bool ret = false;
        if(x<0){
            ret = true;
        }
        return ret;
    }
}
