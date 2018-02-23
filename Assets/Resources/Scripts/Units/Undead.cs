using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undead : Character
{
	protected override void Start()
	{
		name = "Undead";
		faction = (int)FACTIONS.DARK;
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
		description = "Cute lil zombie/skeleton minons";
		topBarDescription = "Evil doods.\n These minions of the dark come from death";//(need to implement different costs for tile movements)
		

	}
}
