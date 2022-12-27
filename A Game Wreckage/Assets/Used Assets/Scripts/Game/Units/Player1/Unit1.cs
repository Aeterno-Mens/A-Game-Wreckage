using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit1 : BasePlayer
{
    // Start is called before the first frame update
    void Start()    
    {
        this.cost = 150;
        this.stamina = 100;
        this.range = 12.5f;
        this.hp = 250;
        this.attack = 80;
        this.currentstamina = stamina;
        this.atribute = 1;
        this.tier = 1;
        transform.Find("Canvas").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth(hp);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
