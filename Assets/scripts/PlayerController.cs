using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //control player
    public float horizontalMove;
    public float verticalMove;
    private Vector3 playerInput;
    public CharacterController player;
    
    public float playerSpeed;
    public Vector3 movePlayer = Vector3.zero;
    public float gravity = 80f;
    public float fallVelocity=0f;
    public float jumpForce;


    //stats
    public int curHealth;
    public int maxHealth = 5;

    //interfaz
    private float timeCounter;
    public float velocityHealth = 0.1f;
    public int curSprite = 0;


    //cameras
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    private MenuManager menuManager;

    //godmode
    public bool god = false;
    public bool goUp;
    public bool goDown;
    public float flow=5f;


    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        player = GetComponent<CharacterController>();

        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        //movimiento + camara
        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        player.transform.LookAt(player.transform.position + movePlayer);

        //godmode
        if (Input.GetKeyDown("f10"))
        {

            if (!god)
            {
                god = true;
            }
            else
            {
                god = false;
            }
        }
        if (god)
        {
            playerSpeed = 0.25f;
            movePlayer = movePlayer * playerSpeed;
            godMode();

        }
        else
        {
            playerSpeed = 10f;
            movePlayer = movePlayer * playerSpeed;
            setGravity();
            playerSkills();


        }

        player.Move(movePlayer * Time.deltaTime);


        //health

        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
        if(curHealth <= 0)
        {
            die();
        }

        // Animación sprite vidas

        timeCounter += Time.deltaTime;
        if (timeCounter >= velocityHealth)
        {
            
            if(curSprite >=6)
            {
                curSprite = -1;
            }
            curSprite++;
            timeCounter = 0;
        }



    }
    //Funcion para la direccion a la que mira la camara
    public void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }
  
    //Fucion para Jump

    public void playerSkills()
    {
        if(player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
          
        }
    }


    //funcion para la gravedad
    public void setGravity()
    {
       
        
       if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
    }
    
    public void die()
    {
        menuManager.GameOver();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Win")
            menuManager.WinScene();
    }
    public void godMode()
    {

        if (Input.GetKey("space"))
        {
            goUp = true;
            fallVelocity += 1f * Time.deltaTime;
            fallVelocity = Mathf.Clamp(fallVelocity, -1f, 1f);


        }
        else if (Input.GetKey("z"))
        {
            goDown = true;
            fallVelocity -= 1f * Time.deltaTime;
            fallVelocity = Mathf.Clamp(fallVelocity, -1f, 1f);

        }
        else
        {
            fallVelocity -= fallVelocity * flow * Time.deltaTime;

        }
        movePlayer.y = fallVelocity;

        player.Move(movePlayer);
    }
 
}





