using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pegasus : Character
{
	protected override void Start()
	{
		name = "Pegasus";
		base.Start();
		maxHp = 10;
		hp = 10;
		move = 6;
		attkRange = 1;
		attk = 5;
		defense = 0;
		cost = 15;
		canMove = true;
		description = "Mobile units with\nno movement costs";
		topBarDescription = "Winged horses that are able to move incredible distances.\n move over water and mountains for free";//(need to implement different costs for tile movements)
		

	}
}
