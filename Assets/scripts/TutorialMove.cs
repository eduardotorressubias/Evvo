using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class TutorialMove : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject tutorial;
    public GameObject trigger;
    private PlayerController player;
    static private bool tutorial1 = true;
    static private bool tutorial2 = true;
    static private bool tutorial3 = true;
    private PlayerAnimation animator;

    private OptionsMenu pause;
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        pause = FindObjectOfType<OptionsMenu>();
        animator = FindObjectOfType<PlayerAnimation>();
        VideoPlayer.loopPointReached += skip;
    }
    private void Update()
    {
        //Debug.Log("1" + tutorial1 + " 2 " + tutorial2 + " 3 " + tutorial3);
        if (tutorial1 == false && tutorial2 == false && tutorial3 == false)
        {
            animator.OpenDoor();
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if ((player.horizontalMove != 0 && other.tag == "Player" && tutorial1 == true) || (player.verticalMove != 0 && other.tag == "Player" && tutorial1 == true))
        {
            
            tutorial1 = false;
            //Debug.Log("1 " + tutorial1);
            skip2();
        }

        if (Input.GetKey("space") && other.tag=="Player"&& tutorial2==true)
        {
            
            tutorial2 = false;
            //Debug.Log("2 " + tutorial2);
            skip2();
        }
        
        if(Input.GetKeyDown(KeyCode.Mouse0) && other.tag == "Player" && tutorial3==true)
        {
            
            tutorial3 = false;
            //Debug.Log("3" + tutorial3);
            skip2();
            animator.OpenDoor();
        }
    }
    
    void skip(VideoPlayer vp){}
    void skip2()
    {       
        tutorial.SetActive(false);
        //player.PlayTime();
        trigger.SetActive(false);

    }
}
