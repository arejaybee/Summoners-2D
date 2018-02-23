using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Character
{
	protected override void Start()
	{
		name = "Turtle";
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
		description = "shellybro";
		topBarDescription = "Earthy doods.\n That boys shell is hard as a rock!";//(need to implement different costs for tile movements)


	}
}
