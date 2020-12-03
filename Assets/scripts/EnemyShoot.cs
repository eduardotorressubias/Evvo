﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 2f;
    public float lifeDuration = 2f;
    private EnemyController enemy;
    private PlayerController player;

    private float lifeTimer;

    void Start()
    {
        lifeTimer = lifeDuration;
        enemy = FindObjectOfType<EnemyController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //make the bullet move
        transform.position += enemy.agent.transform.forward * speed * Time.deltaTime;
        //destruir el bullet
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Scene")
        {
           
            Destroy(gameObject);
        }

        
    }
}
