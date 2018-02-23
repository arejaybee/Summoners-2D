﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succubus : Character
{
	protected override void Start()
	{
		name = "Succubus";
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
		description = "Sexy vampire lady";
		topBarDescription = "Evil doods.\n THICC";//(need to implement different costs for tile movements)
		

	}
}
