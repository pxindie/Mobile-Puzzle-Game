using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class debug_ui : MonoBehaviour
{
    public UnityEngine.UI.Text debugtext;
    public UnityEngine.UI.Text SpecialDBs;
    public UnityEngine.UI.Text fps_counter;
    public touch_input touchinput;
    public pathScript pathScr;
    public GameObject ball;
    public BallControl ballScr;
    public generalScript Engine;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("ball");
        ballScr = ball.GetComponent<BallControl>();
        touchinput = GetComponent<touch_input>();
        pathScr = GetComponent<pathScript>();
        Engine = GetComponent<generalScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Fps();
        debuging();   
    }
    void debuging(){
        List<string> tex = new List<string>();
        // string target = "\nTarget : " + ballScr.nearBase;
        // string docked =  "\nDocked? : "+ballScr.docked;
        // string gates = "\n UpLineGate : " +pathScr.updateLineGate;
        tex.Add("\nScreenPoint : "+ballScr.screenPoint.x+"//"+ballScr.screenPoint.y);
        tex.Add("\nCouples : "+Engine.wayCouples.Count);
        tex.Add("\nWay : "+Engine.way.Count);
        debugtext.text = texGenerator(tex)+wayClaps();
    }

    void Fps(){
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        fps_counter.text = ((int) current)+"FPS";
    }
    string texGenerator(List<string> a){
        string ret ="";
        for (int i = 0; i < a.Count; i++)
        {
            ret = ret + a.ElementAt(i);
        }
        return ret;
    }
    string wayClaps(){
        string ret = "";
        for (int i = 0; i < Engine.wayCouples.Count; i++)
        {
            ret = ret + "\n" +i+")"+Engine.wayCouples.ElementAt(i)[0]+"-->"+Engine.wayCouples.ElementAt(i)[1];
        }
        return ret;
    }
}
