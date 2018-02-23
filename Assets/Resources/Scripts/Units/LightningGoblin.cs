using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGoblin : Character
{
	protected override void Start()
	{
		name = "LightningGoblin";
		faction = (int)FACTIONS.WEATHER;
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
		description = "birb!";
		topBarDescription = "Weather time\n SHOCKING!.";//(need to implement different costs for tile movements)


	}
}
