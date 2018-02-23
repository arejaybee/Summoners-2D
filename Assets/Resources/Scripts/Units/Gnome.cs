using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : Character
{
	protected override void Start()
	{
		name = "Gnome";
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
		description = "lil dudes";
		topBarDescription = "Earthy doods.\n They just think theyre funny for joining tbh";//(need to implement different costs for tile movements)


	}
}
