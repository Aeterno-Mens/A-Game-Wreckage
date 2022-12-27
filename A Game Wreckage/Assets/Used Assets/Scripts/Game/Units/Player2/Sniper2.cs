using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper2 : BasePlayer2
{
    // Start is called before the first frame update
    void Start()
    {
        this.cost = 400;
        this.stamina = 75;
        this.range = 20.0f;
        this.hp = 150;
        this.attack = 250;
        this.currentstamina = stamina;
        this.atribute = 1;
        this.tier = 2;
        transform.Find("Canvas").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth(hp);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
