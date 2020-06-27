using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class touch_input : MonoBehaviour
{
    private Vector2 pos_S; /* Starting touch position */
    private Vector2 pos_E; /* Ending touch position */
    public static Vector2 delta;
    private float pointMax = 50; //maxmimum touch move get as just pointed
    public string inputType = "None"; // User input is "None" or "Point" or "Swap"
    public Dictionary<string, bool> accessKeys = new Dictionary<string, bool>();
    bool touched = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount>0)
        {
            // Debug.Log("Temas var");
            Touch touch = Input.GetTouch(0);
            TouchPhase phase = touch.phase;
            switch (phase)
            {
                case TouchPhase.Began:
                    pos_S = touch.position;
                    break;
                case TouchPhase.Ended: 
                    pos_E = touch.position;
                    changeAll(true);
                    CalculateDelta(pos_S,pos_E);
                    break;
            }
        }
    }

    void CalculateDelta(Vector2 start,Vector2 end){
        delta.x = end.x - start.x;
        delta.y = end.y - start.y;
    }

    public string getTouchInput(string name){
        try{ accessKeys.Add(name,true); }catch{} // if dont registered yet, registering here
        
        string ret="None";
        if(accessKeys[name]){
            float SubX = Mathf.Abs(pos_E.x - pos_S.x);
            float SubY = Mathf.Abs(pos_E.y - pos_S.y);
            if(SubX < pointMax    && SubY < pointMax){
                ret = "Point";
                accessKeys[name] = false;
            }else{
                ret = "Swap";
                accessKeys[name] = false; // In here accessKeys trade with "swap answer"
            }
        }
        return ret;
    }

    void changeAll(bool newvarible){
        foreach( var kvp in accessKeys.Keys.ToList() )
        {
            accessKeys[kvp.ToString()]=newvarible;
        }
    }


}