using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class TutorialMove : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject tutorial;
    private PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        VideoPlayer.loopPointReached += skip;

    }
    private void Update()
    {
        if (Input.GetKey("space"))
        {
            skip2();
        }
    }
    void skip(VideoPlayer vp)
    {
     
 
         
    }
    void skip2()
    {
        tutorial.SetActive(false);
        player.PlayTime();

    }
}
