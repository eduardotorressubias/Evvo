using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public float soundTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Destroy(gameObject, soundTimer);
    }
}
