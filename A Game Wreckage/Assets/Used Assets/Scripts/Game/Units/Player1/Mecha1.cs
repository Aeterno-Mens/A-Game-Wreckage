using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecha1 : BasePlayer
{
    // Start is called before the first frame update
    void Start()    
    {
        this.hp = 650;
        this.stamina = 335;
        this.attack = 410;
        this.currentstamina = stamina;
        this.atribute = 3;
        this.range = 10.0f;//3.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
