using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Character
{

    // Use this for initialization
    protected override void Start()
    {
		base.Start();
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
		if (playerNumber == 1)
		{
			if (hub.summoner1.mana <= 0)
			{
				Destroy(this);
			}
		}
		else if (playerNumber == 2)
		{
			if (hub.summoner2.mana <= 0)
			{
				Destroy(this);
			}
		}
	}

    //dragons drain a player's mana at the end of a turn
    public override void EndTurn()
    {
		if(playerNumber == 1)
		{
			hub.summoner1.mana -= 30;
		}
		else if (playerNumber == 2)
		{
			hub.summoner2.mana -= 30;
		}

	}
}
