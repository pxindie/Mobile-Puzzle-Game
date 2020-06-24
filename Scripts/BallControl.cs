using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallControl : MonoBehaviour
{
    public UnityEngine.UI.Text debugtext;
    public UnityEngine.UI.Text fps_counter;
	public static float ivmeR = 0.5f; // reset acceleration to default
	public float ivme = ivmeR; // acceleration
	public static float dragcof = 0.99f; // acceleration
	public float stopLimit = 0.02f; // acceleration
    public float gravityCoef = 0.0001f;
    private Vector2 speed;
	private Rigidbody2D ball;
    private touch_input touchinput;
    public int nearBase = -2;
    public bool docked = false;
    GameObject[] bases;
    public List<int> way = new List<int>();
    public List<int[]> connect = new List<int[]>();
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
        if (touchinput.getTouchType()=="Swap")
        {
            docked=false;
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
        }
        speed.x = ivme*touch_input.delta.x;
        speed.y = ivme*touch_input.delta.y;
        gravity();
        ball.velocity = new Vector2(speed.x,speed.y);
        debuging();
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
    void debuging(){
        debugtext.text = "Speed : " + speed.x + " " + speed.y + "\nTarget : " + nearBase + "\nDocked? : " +docked;
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        fps_counter.text = ((int) current)+"FPS";
    }
    private void OnTriggerEnter2D(Collider2D other) {
        ivme = 0;
        docked = true;
        way.Add(nearBase);
        if(way.Count>1)
        {
            int[] newConnect = {way[way.Count-2],way[way.Count-1]};
            connect.Add(newConnect);
        }
    }
    void DockConnect(){
        float dockdeltaX = ball.position.x -bases[nearBase].transform.position.x;
        float dockdeltaY = ball.position.y -bases[nearBase].transform.position.y;
            speed.x = (dockdeltaX<0? dockdeltaX>-1:dockdeltaX<1)    ? 0f:-dockdeltaX*15f; //when ball come from negative side of base, cant move to the center so there first dedect come from negative or positive side then calculate near enough
            speed.y = (dockdeltaY<0?dockdeltaY>-1:dockdeltaY<1) ? 0f:-dockdeltaY*15f;
        if( (dockdeltaX<0?dockdeltaX>-1:dockdeltaX<1)  &&  (dockdeltaY<0?dockdeltaY>-1:dockdeltaY<1)   ){docked=true;}
    }
    void drawLines(){ // this void not completed yet
        for (int i = 0; i < way.Count; i++)
        {
            
        }
    }
}