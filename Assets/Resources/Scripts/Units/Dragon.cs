using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Character
{

    // Use this for initialization
    protected override void Start()
    {
        name = "Dragon";
        maxHp = 10;
        hp = 10;
        move = 6;
        attkRange = 2;
        attk = 15;
        defense = 3;
        cost = 75;
        extraDescription = "\n-30 mana per turn\nDies at 0 mana";

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
		if (GameObject.Find("Summoner" + playerNumber + "(clone)").GetComponent<Summoner>().mana <= 0)
		{
			Destroy(this);
		}
	}

    //dragons drain a player's mana at the end of a turn
    public override void EndTurn()
    {
		GameObject.Find("Summoner" + playerNumber + "(clone)").GetComponent<Summoner>().mana -= 30;
	}
}
