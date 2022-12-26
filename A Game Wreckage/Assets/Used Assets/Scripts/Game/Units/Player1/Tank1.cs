using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank1 : BasePlayer
{
    // Start is called before the first frame update
    void Start()
    {
        this.cost = 450;
        this.stamina = 125;
        this.range = 17.5f;
        this.hp = 650;
        this.attack = 175;
        this.currentstamina = stamina;
        this.atribute = 1;
        this.tier = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
