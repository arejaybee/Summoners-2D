using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Character
{
	protected override void Start()
	{
		name = "Demon";
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
		description = "Spooky Boi";
		topBarDescription = "Evil doods.\n These servants to darkness protect their allies!";//(need to implement different costs for tile movements)
		

	}
}
