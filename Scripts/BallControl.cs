using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallControl : MonoBehaviour
{
	public static float ivmeR = 0.05f; // acceleration
	public float ivme = 0.05f; // acceleration
	public static float dragcof = 0.99f; // acceleration
	public float stopLimit = 0.001f; // acceleration
    private Vector2 speed;
	private Rigidbody2D ball;
    private touch_input touchinput;
    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponent<Rigidbody2D> ();
        touchinput = GetComponent<touch_input>();
        // touch_input = GameObject.Find("touch_input.cs").GetComponent<ItemsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // string touchtype = touchinput.getTouchType;
        if (touchinput.getTouchType()!="Swap")
        {
            ivme=ivmeR;
            touchinput.touched = false;
        }

        if (speed.x!=0||speed.y!=0)
        {
            if(ivme < stopLimit) {
                ivme = 0;
            } else { 
                ivme = ivme*dragcof;
            }
            speed.x = ivme*touch_input.delta.x;
            speed.y = ivme*touch_input.delta.y;
            ball.velocity = new Vector2(speed.x,speed.y);
        }
    }
}