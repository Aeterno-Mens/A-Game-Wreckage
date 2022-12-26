using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout2 : BasePlayer2
{
    // Start is called before the first frame update
    void Start()
    {
        this.cost = 350;
        this.stamina = 150;
        this.range = 12.5f;
        this.hp = 200;
        this.attack = 120;
        this.currentstamina = stamina;
        this.atribute = 2;
        this.tier = 2;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
