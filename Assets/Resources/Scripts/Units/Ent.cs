using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent : Character
{
	protected override void Start()
	{
		name = "Ent";
		faction = (int)FACTIONS.EARTH;
		base.Start();
		maxHp = 10;
		hp = 10;
		move = 10;
		attkRange = 10;
		attk = 10;
		defense = 10;
		cost = 10;
		zeal = 10;
		canMove = true;
		description = "BIG boi";
		topBarDescription = "Earthy doods.\n This is literally a tree...";//(need to implement different costs for tile movements)


	}
}
