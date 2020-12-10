using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //control player
    public float horizontalMove;
    public float verticalMove;
    private Vector3 playerInput;
    public CharacterController player;
    
    public float playerSpeed=10f;
    public float godSpeed = 0.25f;
    public Vector3 movePlayer = Vector3.zero;
    public float gravity = 80f;
    public float fallVelocity=0f;
    public float jumpForce;


    //stats
    public int curHealth;
    public int maxHealth = 3;
    public int dmg = 0;
    private bool coldown = false;
    public float cdTime = 0.6f;


    //interfaz
    private float timeCounter;
    private float timeCounterCd = 0;
    public float velocityHealth = 0.1f;
    public int curSprite = 0;
    public GameObject[] go;
    private OptionsMenu optionsMenu;



    //cameras
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 camPosition;
    public GameObject camCaida;
    public GameObject camPlayer;
    private MenuManager menuManager;


    //Attack
    
    public GameObject attackbox;
    public AudioSource attack;
    public AudioSource salto;
    private bool attacking = false;

    public GameObject sonidoSalto;
    public GameObject sonidoAttack;


    //godmode
    public bool god = false;
    public bool goUp;
    public bool goDown;
    public float flow=5f;
    private object audioPlayer;


    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
    
        player = GetComponent<CharacterController>();
        optionsMenu = FindObjectOfType<OptionsMenu>();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;




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

        camPosition = new Vector3 (camPlayer.transform.position.x, camPlayer.transform.position.y, camPlayer.transform.position.z);

        //cursor visible
        if (Input.GetKey(KeyCode.Escape))
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
            

        //attack
        if (Input.GetKeyDown(KeyCode.Mouse0) && optionsMenu.menuIsOpen == false && attacking == false)
        {
            

            
            Instantiate(sonidoAttack);
            attacking = true;


        }

        if (attacking == true)
        {
            timeCounterCd += Time.deltaTime;
            if (timeCounterCd >= cdTime)
            {
                timeCounterCd = 0;
                attacking = false;
                attackbox.SetActive(false);
            }
            else
            {
                attackbox.SetActive(true);
            }
        }
        
     

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
            
            movePlayer = movePlayer * godSpeed;
            godMode();

        }
        else
        {
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
        

        // Animación sprite vidas

        timeCounter += Time.deltaTime;
        if (timeCounter >= velocityHealth)
        {
            
            if (curSprite >=6)
            {
                curSprite = -1;
            }
            curSprite++;
            timeCounter = 0;
        }

        //Start coldown

        if(coldown == true)
        {
            timeCounterCd += Time.deltaTime;
            if(timeCounterCd >= cdTime)
            {
                timeCounterCd = 0;
                coldown = false;
            }
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
            Instantiate(sonidoSalto);
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
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        menuManager.GameOver();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Win" && god == false)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            menuManager.WinScene();
        }
        if(coldown == false && god ==false)
            {
                if (other.tag == "Projectil")
                {
                    damage(1);
                    coldown = true;

                }
                

        }
        if (other.tag == "Lose" && god == false)
        {

            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            menuManager.GameOver();
        }
           
        if (other.tag == "CamaraCaida" && god == false)
        {
            camPlayer.SetActive(true);
           
            camCaida.SetActive(false);
        }



    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CamaraCaida" && god == false)
        {
            camPlayer.SetActive(false);

            camCaida.transform.position = camPosition;
            camCaida.SetActive(true);
        }
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

    public void damage(int dmg)
    {
        curHealth = curHealth - dmg;
        if (curHealth < 0)
        {
            die();
        }
        else if(curHealth == 0){
            
            go[curHealth].SetActive(false);
            die();
        }
        else
        {
            UnityEngine.Debug.Log("vidas = "+ curHealth);
            go[curHealth].SetActive(false);

        }
    }
 
}





