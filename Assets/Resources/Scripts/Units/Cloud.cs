using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : Character
{
	protected override void Start()
	{
		name = "Cloud";
		faction = (int)FACTIONS.WEATHER;
		base.Start();
		maxHp = 10;
		hp = 10;
		move = 7;
		attkRange = 7;
		attk = 10;
		defense = 10;
		cost = 10;
		zeal = 10;
		canMove = true;
		description = "its a cloud!";
		topBarDescription = "Weather time\n An unlikely ally, but hes so happy to be here.";//(need to implement different costs for tile movements)


	}
}
