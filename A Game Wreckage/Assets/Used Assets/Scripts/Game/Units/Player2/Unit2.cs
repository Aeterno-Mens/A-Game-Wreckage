using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit2 : BasePlayer2
{
    // Start is called before the first frame update
    void Start()
    {
        this.hp = 250;
        this.stamina = 75;
        this.attack = 80;
        this.currentstamina = stamina;
        this.atribute = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
