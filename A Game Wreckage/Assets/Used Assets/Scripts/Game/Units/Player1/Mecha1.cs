using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecha1 : BasePlayer
{
    // Start is called before the first frame update
    void Start()    
    {
        this.cost = 1000;
        this.currentstamina = stamina;
        this.atribute = 1;
        this.hp = 750;
        this.stamina = 335;
        this.attack = 410;
        this.currentstamina = stamina;
        this.atribute = 3;
        this.range = 25.0f;
        this.tier = 4;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
