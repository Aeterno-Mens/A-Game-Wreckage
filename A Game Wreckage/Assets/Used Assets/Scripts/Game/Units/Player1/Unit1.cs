using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit1 : BasePlayer
{
    // Start is called before the first frame update
    void Start()    
    {
        this.hp = 250;
        this.stamina = 75;
        this.attack = 80;
        this.currentstamina = stamina;
        this.atribute = 1;
        this.range = 20.0f;//3.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
