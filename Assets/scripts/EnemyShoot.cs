using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyShoot : MonoBehaviour
{

    public float speed;

    private PlayerController evvo;
    private EnemyController enemy;
    private Vector3 target;
    private Vector3 rotation;
    private Vector3 rotation2;
   
    Quaternion rot2;
    Quaternion rot1;
    public Quaternion currentAngle;
    void Start()
    {
        evvo = FindObjectOfType<PlayerController>();
        enemy = FindObjectOfType<EnemyController>();
        target = new Vector3(evvo.player.transform.position.x, evvo.player.transform.position.y+0.7f, evvo.player.transform.position.z);


        rotation = new Vector3(evvo.player.transform.position.x, enemy.AlturaEnemigo+1.2f, evvo.player.transform.position.z);
        //rot1 = Quaternion.LookRotation(rotation);
        //rotation2 = new Vector3(evvo.player.transform.position.x, enemy.AlturaEnemigo -10f, evvo.player.transform.position.z);
        //rot2 = Quaternion.LookRotation(rotation2);

        transform.LookAt(rotation);
        //transform.rotation = rot1;


    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot2, 1f );

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Scene" || other.tag == "lose" || other.tag == "win")
        {
            
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    //public float speed = 2f;
    //public float lifeDuration = 2f;
    //private EnemyController enemy;
    //private PlayerController player;

    //private float lifeTimer;

    //void Start()
    //{
    //    lifeTimer = lifeDuration;
    //    enemy = FindObjectOfType<EnemyController>();

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //make the bullet move
    //    transform.position += enemy.agent.transform.position * speed * Time.deltaTime;
    //    //transform.rotation = Quaternion.LookRotation(enemy.agent.transform.forward);
    //    //destruir el bullet
    //    lifeTimer -= Time.deltaTime;
    //    if(lifeTimer <= 0f)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Player" || other.tag == "Scene")
    //    {

    //        Destroy(gameObject);
    //    }


    //}
}
