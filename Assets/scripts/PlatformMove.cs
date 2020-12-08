using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float travelTime;

    private Rigidbody rb;
    private Vector3 currentPos;
    private PlayerController player;

    CharacterController cc;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = Vector3.Lerp(startPoint.position, endPoint.position, Mathf.Cos(Time.time / travelTime * Mathf.PI * 2) * -.5f + .5f);

        rb.MovePosition(currentPos);
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cc = other.GetComponent<CharacterController>();
            
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerSkills();
            cc.Move(rb.velocity * Time.deltaTime);
        }
    }
    public void playerSkills()
    {
        
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            player.fallVelocity = player.jumpForce;
            player.movePlayer.y = player.fallVelocity;

        }
    }

}
