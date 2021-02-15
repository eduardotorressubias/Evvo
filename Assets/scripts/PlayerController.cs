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


    //public Rigidbody rb;
    //public float distToGround;


    public float playerSpeed = 10f;
    public float godSpeed = 0.25f;
    public Vector3 movePlayer = Vector3.zero;
    public float gravity = 80f;
    public float fallVelocity = 0f;
    public float jumpForce;


    //stats
    public int curHealth;
    public int maxHealth = 3;
    public int dmg = 0;
    private bool coldown = false;
    public float cdTime = 0.6f;
    public bool consumir = false;


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
    private bool attacking = false;
    public GameObject sonidoSalto;
    public GameObject sonidoAttack;


    //godmode
    public bool god = false;
    public bool goUp;
    public bool goDown;
    public float flow = 5f;

    //particles

    public ParticleSystem dust;

    //Tutorial
    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;
    public float oldtimescale = 0f;


    void Start()
    {
       
        menuManager = FindObjectOfType<MenuManager>();
        player = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        optionsMenu = FindObjectOfType<OptionsMenu>();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        curHealth = maxHealth;

       
    }

    void Update()
    {
        // Movimiento

        movement();

        //movimiento + camara

        camDirection();

        //cursor visible

        visibleCursor();

        //attack

        atack();

        //godmode

        checkGod();

        //health

        spriteHealth();

        //Start coldown

        playerColdown();



    }

    //cursor visible y que no se salga de la pantalla 
    public void visibleCursor()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
    }

    //Ataque y coldown de ataque
    public void atack()
    {
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
    }

    //Movimiento wasd
    public void movement()
    {

        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

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

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        player.transform.LookAt(player.transform.position + movePlayer);

        camPosition = new Vector3(camPlayer.transform.position.x, camPlayer.transform.position.y, camPlayer.transform.position.z);
    }

    //Fucion para Jump
    public void playerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            Instantiate(sonidoSalto);
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
            

        }
        else if (!player.isGrounded)
        {
            CreateDust();
        }
       
    }

    //vida y sprite vida
    public void spriteHealth()
    {
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }


        // Animación sprite vidas

        timeCounter += Time.deltaTime;
        if (timeCounter >= velocityHealth)
        {

            if (curSprite >= 6)
            {
                curSprite = -1;
            }
            curSprite++;
            timeCounter = 0;
        }
    }

    //setear coldown de ataque/ daño recibido etc
    public void playerColdown()
    {
        if (coldown == true)
        {
            timeCounterCd += Time.deltaTime;
            if (timeCounterCd >= cdTime)
            {
                timeCounterCd = 0;
                coldown = false;
            }
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

    //muerte del jugador
    public void die()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        menuManager.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Cuando lleguemos a la parte donde se gana, y el god mode sea falso setearemos el cursor visible 
        //y que pueda salir de la ventana del juego, seguidamente pasamos a la escena de win
        if (other.tag == "Win" && god == false)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            menuManager.WinScene();
        }

        //si el jugador recibe daño cuando haya pasado el coldown que reciba daño setee el coldown a 0 otra vez y haga una animación el player
        if (coldown == false && god == false)
        {
            if (other.tag == "Projectil")
            {
                
                damage(1);
                coldown = true;

            }


        }

        // Si el jugador muere, el cursor se muetra visible , y lo puedes mover fuera de la ventana, seguidamente pasa a la escena de gameover
        if (other.tag == "Lose" && god == false)
        {

            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            menuManager.GameOver();
        }

        //Si el jugador cae al vacio y entra en el trigger CamaraCaida se desactivara la camara actual de tercera persona y se activara una que simplemente mire al jugador pero no lo siga
        if (other.tag == "CamaraCaida" && god == false)
        {
            camPlayer.SetActive(true);

            camCaida.SetActive(false);
        }

        //Activar tutorial 1
        if (other.tag == "Tutorial1")
        {
            tutorial1.SetActive(true);
            //oldtimescale = Time.timeScale;
            //Time.timeScale = 0f;
        }
        if (other.tag == "Tutorial2")
        {
            tutorial2.SetActive(true);
            //oldtimescale = Time.timeScale;
            //Time.timeScale = 0f;
        }
        if (other.tag == "Tutorial3")
        {
            tutorial3.SetActive(true);
            //oldtimescale = Time.timeScale;
            //Time.timeScale = 0f;
        }



    }

    private void OnTriggerExit(Collider other)
    {
        //Si el jugador cae al vacio y entra en el trigger CamaraCaida se desactivara la camara actual de tercera persona y se activara una que simplemente mire al jugador pero no lo siga 
        if (other.tag == "CamaraCaida" && god == false)
        {
            camPlayer.SetActive(false);

            camCaida.transform.position = camPosition;
            camCaida.SetActive(true);
        }
    }

    //chequear si es godmode o no
    public void checkGod()
    {
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
        //rb.velocity = ((movePlayer * Time.deltaTime) - rb.position) / Time.fixedDeltaTime;

    }

    //controles de el godmode
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

    //shake camera, quitar vida al jugador, y cuando la vida sea 0 que pase a la muerte del jugador
    public void damage(int dmg)
    {
        CameraShake.Instance.ShakeCamera(5f, .2f);
        curHealth = curHealth - dmg;
        if (curHealth < 0)
        {
            die();
        }
        else if (curHealth == 0)
        {

            go[curHealth].SetActive(false);
            die();
        }
        else
        {
            UnityEngine.Debug.Log("vidas = " + curHealth);
            go[curHealth].SetActive(false);

        }
    }

    public void CreateDust()
    {
        dust.Play();
    }
    

    public void SumarVida()
    {
     
        if (curHealth < 3 && curHealth > 0)
        {
            curHealth = curHealth + 1;
            go[curHealth-1].SetActive(true);
            consumir = true;
        }
        else
        {
            consumir = false;
        }


    }
}








