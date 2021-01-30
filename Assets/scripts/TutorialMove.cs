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

    private OptionsMenu pause;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        pause = FindObjectOfType<OptionsMenu>();
        VideoPlayer.loopPointReached += skip;
    }

    private void OnTriggerStay(Collider other)
    {

        if ((player.horizontalMove != 0 && other.tag == "Player" && tutorial1 == true) || (player.verticalMove != 0 && other.tag == "Player" && tutorial1 == true))
        {
            Debug.Log("1 "+tutorial1);
            tutorial1 = false;
           
            skip2();
        }

        if (Input.GetKey("space") && other.tag=="Player"&& tutorial2==true)
        {
            Debug.Log("2 "+tutorial2);
            tutorial2 = false;
            
            skip2();
        }
        
        if(Input.GetKeyDown(KeyCode.Mouse0) && other.tag == "Player" && tutorial3==true)
        {
            Debug.Log("3" +tutorial3);
            tutorial3 = false;
            
            skip2();
        }
    }
    
    void skip(VideoPlayer vp){}
    void skip2()
    {
        //if (pause.menuIsOpen)
        //{
        //    tutorial.SetActive(false);
        //}
       

        tutorial.SetActive(false);
        //player.PlayTime();
        trigger.SetActive(false);


    }
}
