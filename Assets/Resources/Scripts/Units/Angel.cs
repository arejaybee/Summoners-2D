using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : Character
{
	protected override void Start()
	{
		name = "Angel";
		faction = (int)FACTIONS.LIGHT;
		base.Start();
		maxHp = 10;
		hp = 10;
		move = 9;
		attkRange = 10;
		attk = 10;
		defense = 10;
		cost = 10;
		zeal = 10;
		canMove = true;
		description = "MERCY";
		topBarDescription = "Devout followers of the holy light.\n They heal their allies with radiant light.";//(need to implement different costs for tile movements)
		

	}
}
