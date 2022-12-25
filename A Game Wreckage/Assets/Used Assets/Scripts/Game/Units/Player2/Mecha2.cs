using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecha2 : BasePlayer2
{
    // Start is called before the first frame update
    void Start()
    {
        this.hp = 750;
        this.stamina = 225;
        this.attack = 350;
        this.currentstamina = stamina;
        this.atribute = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
