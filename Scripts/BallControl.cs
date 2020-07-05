using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{ 
	public static float ivmeR = 0.5f; // reset acceleration to defaul
	public  float ivmeX = ivmeR,ivmeY = ivmeR; // reset acceleration to defaul
    private Vector2 speed;
	private Rigidbody2D ball;
    private touch_input touchinput;
    private generalScript Engine;
    private pathScript pathScr;
    public int nearBase = -2;
    GameObject[] bases;
    GameObject deepmind;
    public Vector2 screenPoint;
    public bool docked,dockaccess,dockinput;

    // Start is called before the first frame update
    void Start()
    {
        deepmind = GameObject.Find("DeepMind");
        ball = GetComponent<Rigidbody2D>();
        touchinput = deepmind.GetComponent<touch_input>();
        pathScr = deepmind.GetComponent<pathScript>();
        Engine = deepmind.GetComponent<generalScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!Engine.fail&&!Engine.win){
            // string touchtype = touchinput.getTouchType;
            if (touchinput.getTouchInput("BallControl")=="Swap"){
                ivmeX = ivmeR;
                ivmeY = ivmeR;
                docked=false;
            }
        }
        bounce();
        speed.x = ivmeX*touch_input.delta.x;
        speed.y = ivmeY*touch_input.delta.y;
        gravity();
        ball.velocity = new Vector2(speed.x,speed.y);
    }

    void gravity(){
        bases = GameObject.FindGameObjectsWithTag("base");
        if(!docked){ //kenetlenmişse hesaplamakla uğraşmıyor
            float[] basedata = {-1,100000,0,0,0};
            for (int i = 0; i < bases.Length; i++)
            {
                if(true){ // dock'lu olan ayıklanıyor
                    float subX = ball.position.x - bases[i].transform.position.x;
                    float subY = ball.position.y - bases[i].transform.position.y; 
                    if(basedata[1]>getHippo(subX,subY)){
                        basedata[0] = i; // base id
                        nearBase = i;
                        basedata[1]=Mathf.Abs(getHippo(subX,subY)); // ball and base distance 
                        basedata[2]=subX; // x axis distance
                        basedata[3]=subY; // y axis distance
                    }
                }
            }
        }
        if(docked){DockConnect();}
    }

    public float getHippo(float x,float y){
        return Mathf.Sqrt(Mathf.Pow(x,2)+Mathf.Pow(y,2));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ivmeX = 0;
        ivmeY = 0;
        docked = true;
        Engine.way.Add(nearBase);
        if(Engine.way.Count>1){
            if(!(Engine.way[Engine.way.Count-2]==nearBase)){
                int[] couple = {(int)Engine.way[Engine.way.Count-2],(int)Engine.way[Engine.way.Count-1]};
                Engine.wayCouples.Add(couple);
                Engine.checkFailure();
            }else{Engine.way.RemoveAt(Engine.way.Count-1);}
        }
    }
    void DockConnect(){
        float dockdeltaX = ball.position.x -bases[nearBase].transform.position.x;
        float dockdeltaY = ball.position.y -bases[nearBase].transform.position.y;
            speed.x = (dockdeltaX<0? dockdeltaX>-1:dockdeltaX<1)    ? 0f:-dockdeltaX*15f; //when ball come from negative side of base, cant move to the center so there first dedect come from negative or positive side then calculate near enough
            speed.y = (dockdeltaY<0?dockdeltaY>-1:dockdeltaY<1) ? 0f:-dockdeltaY*15f;
        if( (dockdeltaX<0?dockdeltaX>-1:dockdeltaX<1)  &&  (dockdeltaY<0?dockdeltaY>-1:dockdeltaY<1)   ){docked=true;}
    }
    void bounce(){
        float deltaX = touch_input.delta.x;
        float deltaY = touch_input.delta.y;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if(screenPoint.x<5)
        {
            ivmeX = Engine.IsNegative(deltaX) ? -Mathf.Abs(ivmeX) : Mathf.Abs(ivmeX);
        }else if(screenPoint.x>Screen.width-5)
        {
            ivmeX = Engine.IsNegative(deltaX) ? Mathf.Abs(ivmeX) : -Mathf.Abs(ivmeX);
        }

        if(screenPoint.y<5)
        {
            ivmeY = Engine.IsNegative(deltaY) ? -Mathf.Abs(ivmeY) : Mathf.Abs(ivmeY);
        }else if(screenPoint.y>Screen.height-5)
        {
            ivmeY = Engine.IsNegative(deltaY) ? Mathf.Abs(ivmeY) : -Mathf.Abs(ivmeY);
        }
    }
}