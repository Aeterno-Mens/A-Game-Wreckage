using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger1 : BasePlayer
{
    // Start is called before the first frame update
    void Start()
    {
        this.cost = 350;
        this.stamina = 200;
        this.range = 7.5f;
        this.hp = 450;
        this.attack = 275;
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
