using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rex : Character {

	protected override void Start()
	{
		name = "Rex";
		base.Start();
		maxHp = 25;
		hp = 25;
		move = 4;
		attkRange = 1;
		attk = 10;
		defense = 2;
		cost = 50;
		zeal = 5;
		canMove = true;
		description = "King of dinosaurs!";
		topBarDescription = "King of all dinosaures; the T-rex will crush your foes.";
	}
}
