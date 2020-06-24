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
    private float pointMax = 50; //maxmimum touch move get as just pointed
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
                    // angle = getAngle(pos_E);
                    // Debug.Log(angle);
                    touched = true;
                    CalculateDelta(pos_S,pos_E);
                    break;
            }
        }
    }

    void CalculateDelta(Vector2 start,Vector2 end){
        delta.x = end.x - start.x;
        delta.y = end.y - start.y;
    }
    public double getAngle(Vector2 posE){
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
            float SubX = Mathf.Abs(pos_E.x - pos_S.x);
            float SubY = Mathf.Abs(pos_E.y - pos_S.y);
            if(SubX < pointMax    && SubY < pointMax){
                ret = "Point";
                Debug.Log("Pointed");
                // Debug.Log("X" +pos_E.x + "\nY"+pos_E.y);
                touched=false;
            }else{
                ret = "Swap";
                Debug.Log("Swapped");
            }
        }
        return ret;
    }
}