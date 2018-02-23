using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid : Character
{
	protected override void Start()
	{
		name = "Druid";
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
		description = "hippie";
		topBarDescription = "Earthy doods.\n They make salves!!!! Buy one get one free fam.";//(need to implement different costs for tile movements)


	}
}
