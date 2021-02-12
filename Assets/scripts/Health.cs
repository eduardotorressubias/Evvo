﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Audio;
    private PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {
            player.SumarVida();
            if (player.consumir == true)
            {
                Audio.Play();
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
