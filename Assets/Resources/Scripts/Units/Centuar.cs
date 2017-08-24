using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centuar : Character {

	protected override void Start()
	{
		base.Start();
		name = "Centuar";
		maxHp = 10;
		hp = 10;
		move = 5;
		attkRange = 2;
		attk = 3;
		defense = 0;
		cost = 7;
		zeal = 20;
		piercing = true;
		description = "Mobile range units\nThat can pierce defences";
		topBarDescription = "This beast is able to move around quickly.\nWith their trusty bows they strike at a distance.";
		canMove = true;
	}
}
