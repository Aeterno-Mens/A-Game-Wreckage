using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane2 : BasePlayer2
{
    // Start is called before the first frame update
    void Start()
    {
        this.cost = 500;
        this.stamina = 225;
        this.range = 15.0f;
        this.hp = 500;
        this.attack = 175;
        this.currentstamina = stamina;
        this.atribute = 3;
        this.tier = 3;
        transform.Find("Canvas").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth(hp);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
