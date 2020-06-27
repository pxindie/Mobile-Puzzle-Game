using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BallControl : MonoBehaviour
{
    public UnityEngine.UI.Text failedmessage;
	public static float ivmeR = 0.5f; // reset acceleration to defaul
	public  float ivme = ivmeR; // reset acceleration to defaul
    private Vector2 speed;
	private Rigidbody2D ball;
    private touch_input touchinput;
    public int nearBase = -2;
    GameObject[] bases;
    GameObject deepmind;
    public List<int> way;
    public List<int[]> wayCouples = new List<int[]>();
    public bool docked,dockaccess,dockinput;

    // Start is called before the first frame update
    void Start()
    {
        deepmind = GameObject.Find("DeepMind");
        ball = GetComponent<Rigidbody2D> ();
        touchinput = deepmind.GetComponent<touch_input>();
    }

    // Update is called once per frame
    void Update()
    {
        // string touchtype = touchinput.getTouchType;
        if (touchinput.getTouchInput("BallControl")=="Swap"){
            ivme = ivmeR;
            docked=false;
        }
        speed.x = ivme*touch_input.delta.x;
        speed.y = ivme*touch_input.delta.y;
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
        ivme = 0;
        docked = true;
        way.Add(nearBase);
        if(way.Count>1){
            int[] couple = {(int)way[way.Count-1],(int)way[way.Count-2]};
            // Debug.Log(couplea[0]+couplea[1].ToString());
            // Debug.Log(coupleb[0]+coupleb[1].ToString());
            wayCouples.Add(couple);
        }
        checkFailure(); 
        // Debug.Log(way[way.Count-1]);
    }
    void DockConnect(){
        float dockdeltaX = ball.position.x -bases[nearBase].transform.position.x;
        float dockdeltaY = ball.position.y -bases[nearBase].transform.position.y;
            speed.x = (dockdeltaX<0? dockdeltaX>-1:dockdeltaX<1)    ? 0f:-dockdeltaX*15f; //when ball come from negative side of base, cant move to the center so there first dedect come from negative or positive side then calculate near enough
            speed.y = (dockdeltaY<0?dockdeltaY>-1:dockdeltaY<1) ? 0f:-dockdeltaY*15f;
        if( (dockdeltaX<0?dockdeltaX>-1:dockdeltaX<1)  &&  (dockdeltaY<0?dockdeltaY>-1:dockdeltaY<1)   ){docked=true;}
    }

    void checkFailure()
    {
        Debug.Log("Failure Checked");
        if(way.Count>1){
            if(way[way.Count-1]==way[way.Count-2])    // first failure if re dock same base
            {
                failure();
            }
            if(checkSameItem()) // follow the same path again
            {
                failure();
            }
        }
        if(false) //ball escape from the screen
        {}
    }

    void failure(){
        Debug.Log("**********************YOU FAILED**********************");
        failedmessage.text = "YOU FAILED";
    }

    bool checkSameItem(){
        for (int i = 0; i < wayCouples.Count; i++)
        {
            for (int u = i+1; u < wayCouples.Count; u++)
            {
                Debug.Log("Control List : "+i+" -- "+u+" ("+wayCouples.Count+")");
                bool fullySame = wayCouples.ElementAt(i)[0]==wayCouples.ElementAt(u)[0]&&wayCouples.ElementAt(i)[1]==wayCouples.ElementAt(u)[1];
                bool reverseSame = wayCouples.ElementAt(i)[0]==wayCouples.ElementAt(u)[1]&&wayCouples.ElementAt(i)[1]==wayCouples.ElementAt(u)[0];
                if(fullySame||reverseSame){ return true;}
            }
        }
        return false;
    }
}