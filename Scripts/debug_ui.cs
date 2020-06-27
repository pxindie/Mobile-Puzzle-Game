using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug_ui : MonoBehaviour
{
    public UnityEngine.UI.Text debugtext;
    public UnityEngine.UI.Text fps_counter;
    public touch_input touchinput;
    public pathScript pathScr;
    public GameObject ball;
    public BallControl ballScr;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("ball");
        ballScr = ball.GetComponent<BallControl>();
        touchinput = GetComponent<touch_input>();
        pathScr = GetComponent<pathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Fps();
        debuging();   
    }
        void debuging(){
        string target = "\nTarget : " + ballScr.nearBase;
        string docked =  "\nDocked? : "+ballScr.docked;
        string gates = "\n UpLineGate : " +pathScr.updateLineGate;
        // string docked =  "\nDocked? : "+ballScr.docked
        // string docked =  "\nDocked? : "+ballScr.docked
        // string docked =  "\nDocked? : "+ballScr.docked
        // string docked =  "\nDocked? : "+ballScr.docked
        // string docked =  "\nDocked? : "+ballScr.docked

        debugtext.text = target+docked+gates;
    }
    void Fps(){
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        fps_counter.text = ((int) current)+"FPS";
    }
}
