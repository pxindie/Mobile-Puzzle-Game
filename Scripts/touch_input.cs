using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touch_input : MonoBehaviour
{
    private Vector2 pos_S; /* Starting touch position */
    private Vector2 pos_E; /* Ending touch position */
    public static Vector2 delta;
    private float radian = 0f;
    public double angle = 0d;
    public bool touched = false; //for detecting new touch
    public string inputType = "None"; // User input is "None" or "Point" or "Swap"
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount>0)
        {
            Debug.Log("Temas var");
            Touch touch = Input.GetTouch(0);
            TouchPhase phase = touch.phase;
            switch (phase)
            {
                case TouchPhase.Began:
                    pos_S = touch.position;
                    break;
                case TouchPhase.Ended:
                    pos_E = touch.position;
                    angle = getAngle(pos_E);
                    // Debug.Log(angle);
                    touched = true;
                    break;
            }

        }
    }

    public double getAngle(Vector2 posE){
        delta.x = posE.x - pos_S.x;
        delta.y = posE.y - pos_S.y;
        radian = Mathf.Atan2(delta.y,delta.x);
        if (radian<0f)
        {
            radian = Mathf.Abs(radian);
        } 
        else 
        {
            radian = 2f* Mathf.PI - radian;
        }
        return radian*Mathf.Rad2Deg;
    }
    public string getTouchType(){
        string ret="None";
        if(touched){
            float SubX = Mathf.Abs(pos_E.x) - Mathf.Abs(pos_S.x);
            float SubY = Mathf.Abs(pos_E.y) - Mathf.Abs(pos_S.y);
            if(SubX < 20    && SubY < 20){
                ret = "Point";
            }else{
                ret = "Swap";
            }
        }
        return ret;
    }
}