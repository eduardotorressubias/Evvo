﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
   
    public Animator animator;

   
    public void OnDmgComplete()
    {
        animator.SetTrigger("dmg");
    }
}
