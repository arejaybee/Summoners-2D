﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pegasus : Character
{
	protected override void Start()
	{
		name = "Pegasus";
		faction = (int)FACTIONS.LIGHT;
		base.Start();
		maxHp = 10;
		hp = 10;
		move = 6;
		attkRange = 1;
		attk = 5;
		defense = 0;
		cost = 15;
		zeal = 20;
		canMove = true;
		description = "Fast mobile units";
		topBarDescription = "Winged horses that are able to move incredible distances.\n move over water and mountains for free";//(need to implement different costs for tile movements)
		

	}
}
